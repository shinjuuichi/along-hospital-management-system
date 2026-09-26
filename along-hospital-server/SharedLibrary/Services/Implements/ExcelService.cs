using ClosedXML.Excel;
using SharedLibrary.Services.Interfaces;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SharedLibrary.Services.Implements
{
    public class ExcelService : IExcelService
    {
        private const string MetricHeader = "Metric";
        private const string ValueHeader = "Value";
        private const string LabelHeader = "Label";

        private sealed class SectionExportData
        {
            public List<MetricRow> Metrics { get; } = [];
            public List<TableBlock> DistributionTables { get; } = [];
            public List<TableBlock> ChartTables { get; } = [];
            public List<TableBlock> CollectionTables { get; } = [];
            public bool HasContent =>
                Metrics.Count > 0
                || DistributionTables.Count > 0
                || ChartTables.Count > 0
                || CollectionTables.Count > 0;
        }

        private sealed class MetricRow(string label, object? value)
        {
            public string Label { get; } = label;
            public object? Value { get; } = value;
        }

        private sealed class TableBlock(string title, IReadOnlyList<string> headers, IReadOnlyList<IReadOnlyList<object?>> rows)
        {
            public string Title { get; } = title;
            public IReadOnlyList<string> Headers { get; } = headers;
            public IReadOnlyList<IReadOnlyList<object?>> Rows { get; } = rows;
        }

        public async Task<List<Dictionary<string, object>>> ReadExcelToDictionaryAsync(
            Stream fileStream,
            string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (extension != ".xlsx" && extension != ".csv")
            {
                throw new InvalidDataException($"Unsupported file type '{extension}'. Only .xlsx and .csv files are supported.");
            }

            if (extension == ".csv")
            {
                return await ReadCsvToDictionaryAsync(fileStream);
            }

            return await ReadExcelToDictionaryInternalAsync(fileStream);
        }

        public async Task<List<T>> ReadExcelAsync<T>(Stream fileStream, string fileName) where T : class, new()
        {
            var dictionaries = await ReadExcelToDictionaryAsync(fileStream, fileName);
            var result = new List<T>();

            foreach (var dict in dictionaries)
            {
                var item = new T();
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    if (dict.TryGetValue(propertyName, out var value) && value != null)
                    {
                        var convertedValue = ConvertValue(value, property.PropertyType);
                        property.SetValue(item, convertedValue);
                    }
                }

                result.Add(item);
            }

            return result;
        }

        private Task<List<Dictionary<string, object>>> ReadExcelToDictionaryInternalAsync(Stream fileStream)
        {
            return Task.Run(() =>
            {
                var result = new List<Dictionary<string, object>>();

                using var workbook = new XLWorkbook(fileStream);
                var worksheet = workbook.Worksheets.First();

                if (worksheet.RowsUsed().Count() == 0)
                {
                    throw new InvalidDataException("Excel file is empty or has no data.");
                }

                var firstRow = worksheet.FirstRowUsed();
                if (firstRow == null)
                {
                    throw new InvalidDataException("Excel file has no header row.");
                }

                var cellsUsed = firstRow.CellsUsed();
                if (cellsUsed == null || !cellsUsed.Any())
                {
                    throw new InvalidDataException("Excel file header row is empty.");
                }

                var headers = cellsUsed
                    .Select((cell, index) => new { Index = cell.Address.ColumnNumber, Header = cell.GetString().Trim() })
                    .ToDictionary(x => x.Index, x => x.Header);

                var dataRows = worksheet.RowsUsed().Skip(1);

                foreach (var row in dataRows)
                {
                    var rowData = new Dictionary<string, object>();

                    foreach (var header in headers)
                    {
                        var cell = row.Cell(header.Key);
                        var value = GetCellValue(cell);
                        if (value != null || !string.IsNullOrWhiteSpace(header.Value))
                        {
                            rowData[header.Value] = value ?? string.Empty;
                        }
                    }

                    if (rowData.Values.Any(v => !string.IsNullOrWhiteSpace(v?.ToString())))
                    {
                        result.Add(rowData);
                    }
                }

                return result;
            });
        }

        private async Task<List<Dictionary<string, object>>> ReadCsvToDictionaryAsync(Stream fileStream)
        {
            var result = new List<Dictionary<string, object>>();

            using var reader = new StreamReader(fileStream);
            string? line;
            List<string>? headers = null;
            bool isFirstLine = true;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (isFirstLine)
                {
                    headers = ParseCsvLine(line).Select(h => h.Trim()).ToList();
                    isFirstLine = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line) || headers == null)
                {
                    continue;
                }

                var columns = ParseCsvLine(line);
                var rowData = new Dictionary<string, object>();

                for (int i = 0; i < headers.Count && i < columns.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(headers[i]))
                    {
                        rowData[headers[i]] = columns[i]?.Trim() ?? string.Empty;
                    }
                }

                if (rowData.Values.Any(v => !string.IsNullOrWhiteSpace(v?.ToString())))
                {
                    result.Add(rowData);
                }
            }

            return result;
        }

        private object? GetCellValue(IXLCell cell)
        {
            if (cell.IsEmpty())
            {
                return null;
            }

            return cell.DataType switch
            {
                XLDataType.Text => cell.GetString(),
                XLDataType.Number => cell.GetDouble(),
                XLDataType.DateTime => cell.GetDateTime(),
                XLDataType.Boolean => cell.GetBoolean(),
                XLDataType.TimeSpan => cell.GetTimeSpan(),
                _ => cell.GetString()
            };
        }

        private object? ConvertValue(object value, Type targetType)
        {
            if (value == null)
            {
                return null;
            }

            if (targetType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (value is string stringValue)
            {
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return underlyingType.IsValueType && Nullable.GetUnderlyingType(targetType) == null
                        ? Activator.CreateInstance(underlyingType)
                        : null;
                }

                if (underlyingType == typeof(DateTime) && DateTime.TryParse(stringValue, out var dateTime))
                {
                    return dateTime;
                }
                else if (underlyingType == typeof(int) && int.TryParse(stringValue, out var intValue))
                {
                    return intValue;
                }
                else if (underlyingType == typeof(double) && double.TryParse(stringValue, out var doubleValue))
                {
                    return doubleValue;
                }
                else if (underlyingType == typeof(decimal) && decimal.TryParse(stringValue, out var decimalValue))
                {
                    return decimalValue;
                }
                else if (underlyingType == typeof(bool) && bool.TryParse(stringValue, out var boolValue))
                {
                    return boolValue;
                }

                return stringValue;
            }

            try
            {
                return Convert.ChangeType(value, underlyingType);
            }
            catch
            {
                return value;
            }
        }

        private List<string> ParseCsvLine(string line)
        {
            var columns = new List<string>();
            var currentColumn = new StringBuilder();
            bool inQuotes = false;

            foreach (var c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    columns.Add(currentColumn.ToString());
                    currentColumn.Clear();
                }
                else
                {
                    currentColumn.Append(c);
                }
            }

            columns.Add(currentColumn.ToString());

            return columns;
        }

        public async Task<byte[]> WriteCsvAsync<T>(T data, string title) where T : class
        {
            var headers = new List<string>();
            var values = new List<string>();
            var seenNames = new Dictionary<string, int>();

            FlattenObject(data, headers, values, seenNames);

            var sb = new StringBuilder();

            sb.Append('\uFEFF');

            sb.AppendLine(EscapeCsvField(title));

            sb.AppendLine(string.Join(",", headers.Select(EscapeCsvField)));

            sb.AppendLine(string.Join(",", values.Select(EscapeCsvField)));

            return await Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public async Task<byte[]> WriteSectionedExcelAsync<T>(T data, string title, string? subtitle = null)
            where T : class
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Statistics");

            var currentRow = string.IsNullOrWhiteSpace(subtitle) ? 3 : 4;
            var maxColumn = 2;

            var sectionProperties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in sectionProperties)
            {
                var sectionValue = property.GetValue(data);
                if (sectionValue == null)
                {
                    continue;
                }

                var sectionData = BuildSectionExportData(sectionValue);
                if (!sectionData.HasContent)
                {
                    continue;
                }

                var sectionTitleCell = worksheet.Cell(currentRow, 1);
                sectionTitleCell.Value = HumanizeLabel(property.Name);
                sectionTitleCell.Style.Font.Bold = true;
                sectionTitleCell.Style.Font.FontSize = 13;
                sectionTitleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#DBEAFE");
                currentRow++;

                if (sectionData.Metrics.Count > 0)
                {
                    var metricRows = sectionData.Metrics
                        .Select(metric => (IReadOnlyList<object?>)[metric.Label, metric.Value])
                        .ToList();

                    currentRow = WriteTable(
                        worksheet,
                        currentRow,
                        [MetricHeader, ValueHeader],
                        metricRows);
                    maxColumn = Math.Max(maxColumn, 2);
                    currentRow++;
                }

                foreach (var table in sectionData.DistributionTables)
                {
                    currentRow = WriteTitledTable(worksheet, currentRow, table);
                    maxColumn = Math.Max(maxColumn, table.Headers.Count);
                }

                foreach (var table in sectionData.ChartTables)
                {
                    currentRow = WriteTitledTable(worksheet, currentRow, table);
                    maxColumn = Math.Max(maxColumn, table.Headers.Count);
                }

                foreach (var table in sectionData.CollectionTables)
                {
                    currentRow = WriteTitledTable(worksheet, currentRow, table);
                    maxColumn = Math.Max(maxColumn, table.Headers.Count);
                }

                currentRow++;
            }

            if (maxColumn < 2)
            {
                maxColumn = 2;
            }

            var titleRange = worksheet.Range(1, 1, 1, maxColumn);
            titleRange.Merge();
            titleRange.Value = title;
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 16;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            if (!string.IsNullOrWhiteSpace(subtitle))
            {
                var subtitleRange = worksheet.Range(2, 1, 2, maxColumn);
                subtitleRange.Merge();
                subtitleRange.Value = subtitle;
                subtitleRange.Style.Font.Italic = true;
                subtitleRange.Style.Font.FontSize = 11;
                subtitleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                subtitleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            worksheet.Columns(1, maxColumn).AdjustToContents();

            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return await Task.FromResult(memoryStream.ToArray());
        }

        private void FlattenObject(object? obj, List<string> headers, List<string> values, Dictionary<string, int> seenNames)
        {
            if (obj == null)
            {
                return;
            }

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                var propName = prop.Name;

                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (IsDashboardChartOrDistributionType(propType))
                {
                    continue;
                }

                if (IsSimpleType(propType))
                {
                    if (!seenNames.ContainsKey(propName))
                    {
                        seenNames[propName] = 1;
                        headers.Add(propName);
                        values.Add(value?.ToString() ?? string.Empty);
                    }
                }
                else if (value is System.Collections.IEnumerable && value is not string)
                {
                    if (!seenNames.ContainsKey(propName))
                    {
                        seenNames[propName] = 1;
                        headers.Add(propName);
                        var json = JsonSerializer.Serialize(value);
                        values.Add(json);
                    }
                }
                else
                {
                    FlattenObject(value, headers, values, seenNames);
                }
            }
        }

        private static bool IsDashboardChartOrDistributionType(Type type)
        {
            var ns = type.Namespace;
            return ns == "ReportSvc.BLL.DTOs.DashboardSharedDTOs";
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(Guid)
                || type == typeof(double)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(float)
                || type == typeof(bool);
        }

        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }

            bool needsQuotes = field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r');

            if (needsQuotes)
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }

        private static SectionExportData BuildSectionExportData(object section)
        {
            var result = new SectionExportData();
            var properties = section.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(section);
                if (value == null)
                {
                    continue;
                }

                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var propertyLabel = HumanizeLabel(property.Name);

                if (TryBuildDistributionTable(propertyLabel, value, out var distributionTable))
                {
                    result.DistributionTables.Add(distributionTable);
                    continue;
                }

                if (TryBuildChartTable(propertyLabel, value, out var chartTable))
                {
                    result.ChartTables.Add(chartTable);
                    continue;
                }

                if (IsSimpleType(propertyType))
                {
                    result.Metrics.Add(new MetricRow(propertyLabel, value));
                    continue;
                }

                if (value is IEnumerable enumerable && value is not string)
                {
                    if (TryBuildCollectionTable(propertyLabel, enumerable, out var collectionTable))
                    {
                        result.CollectionTables.Add(collectionTable);
                    }
                }
            }

            return result;
        }

        private static bool TryBuildDistributionTable(string title, object value, out TableBlock table)
        {
            table = null!;

            if (!TryGetPropertyValue(value, "Labels", out var labelsValue) || !TryGetPropertyValue(value, "Data", out var dataValue))
            {
                return false;
            }

            if (TryGetPropertyValue(value, "Datasets", out _))
            {
                return false;
            }

            var labels = ToStringList(labelsValue);
            var data = ToObjectList(dataValue);
            var rowCount = Math.Max(labels.Count, data.Count);
            if (rowCount == 0)
            {
                return false;
            }

            var rows = new List<IReadOnlyList<object?>>();
            for (var index = 0; index < rowCount; index++)
            {
                rows.Add([
                    index < labels.Count ? labels[index] : string.Empty,
                    index < data.Count ? data[index] : null,
                ]);
            }

            table = new TableBlock(title, [LabelHeader, ValueHeader], rows);
            return true;
        }

        private static bool TryBuildChartTable(string title, object value, out TableBlock table)
        {
            table = null!;

            if (!TryGetPropertyValue(value, "Labels", out var labelsValue) ||
                !TryGetPropertyValue(value, "Datasets", out var datasetsValue))
            {
                return false;
            }

            var labels = ToStringList(labelsValue);
            var datasetObjects = ToObjectList(datasetsValue)
                .Where(item => item != null)
                .Cast<object>()
                .ToList();
            if (datasetObjects.Count == 0)
            {
                return false;
            }

            var datasetHeaders = new List<string>();
            var datasetValues = new List<List<object?>>();

            foreach (var dataset in datasetObjects)
            {
                var datasetLabel = TryGetPropertyValue(dataset, "Label", out var rawLabel)
                    ? HumanizeLabel(rawLabel?.ToString() ?? string.Empty)
                    : string.Empty;

                if (string.IsNullOrWhiteSpace(datasetLabel))
                {
                    datasetLabel = $"Value {datasetHeaders.Count + 1}";
                }

                datasetHeaders.Add(datasetLabel);

                if (TryGetPropertyValue(dataset, "Data", out var values))
                {
                    datasetValues.Add(ToObjectList(values));
                }
                else
                {
                    datasetValues.Add([]);
                }
            }

            var rowCount = labels.Count;
            foreach (var dataset in datasetValues)
            {
                rowCount = Math.Max(rowCount, dataset.Count);
            }

            if (rowCount == 0)
            {
                return false;
            }

            var headers = new List<string> { LabelHeader };
            headers.AddRange(datasetHeaders);

            var rows = new List<IReadOnlyList<object?>>();
            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var row = new List<object?> { rowIndex < labels.Count ? labels[rowIndex] : (rowIndex + 1).ToString() };
                foreach (var dataset in datasetValues)
                {
                    row.Add(rowIndex < dataset.Count ? dataset[rowIndex] : null);
                }
                rows.Add(row);
            }

            table = new TableBlock(title, headers, rows);
            return true;
        }

        private static bool TryBuildCollectionTable(string title, IEnumerable enumerable, out TableBlock table)
        {
            table = null!;

            var items = enumerable.Cast<object?>().Where(item => item != null).Cast<object>().ToList();
            if (items.Count == 0)
            {
                return false;
            }

            var itemType = items[0].GetType();
            if (IsSimpleType(itemType))
            {
                var simpleRows = items
                    .Select(item => (IReadOnlyList<object?>)[item])
                    .ToList();

                table = new TableBlock(title, [ValueHeader], simpleRows);
                return true;
            }

            var itemProperties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (itemProperties.Length == 0)
            {
                return false;
            }

            var headers = itemProperties.Select(property => HumanizeLabel(property.Name)).ToList();
            var rows = new List<IReadOnlyList<object?>>();

            foreach (var item in items)
            {
                var row = new List<object?>();
                foreach (var property in itemProperties)
                {
                    var cellValue = property.GetValue(item);
                    var cellType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    if (cellValue == null)
                    {
                        row.Add(null);
                    }
                    else if (IsSimpleType(cellType))
                    {
                        row.Add(cellValue);
                    }
                    else
                    {
                        row.Add(JsonSerializer.Serialize(cellValue));
                    }
                }
                rows.Add(row);
            }

            table = new TableBlock(title, headers, rows);
            return true;
        }

        private static int WriteTitledTable(IXLWorksheet worksheet, int startRow, TableBlock table)
        {
            var titleCell = worksheet.Cell(startRow, 1);
            titleCell.Value = table.Title;
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.FontSize = 11;
            titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#E2E8F0");

            var nextRow = WriteTable(worksheet, startRow + 1, table.Headers, table.Rows);
            return nextRow + 1;
        }

        private static int WriteTable(
            IXLWorksheet worksheet,
            int startRow,
            IReadOnlyList<string> headers,
            IReadOnlyList<IReadOnlyList<object?>> rows)
        {
            for (var columnIndex = 0; columnIndex < headers.Count; columnIndex++)
            {
                var headerCell = worksheet.Cell(startRow, columnIndex + 1);
                headerCell.Value = headers[columnIndex];
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#BFDBFE");
                headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }

            var currentRow = startRow + 1;
            foreach (var row in rows)
            {
                for (var columnIndex = 0; columnIndex < headers.Count; columnIndex++)
                {
                    var cell = worksheet.Cell(currentRow, columnIndex + 1);
                    SetCellValue(cell, columnIndex < row.Count ? row[columnIndex] : null);
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }

                currentRow++;
            }

            return currentRow;
        }

        private static void SetCellValue(IXLCell cell, object? value)
        {
            if (value == null)
            {
                cell.Value = string.Empty;
                return;
            }

            switch (value)
            {
                case int intValue:
                    cell.Value = intValue;
                    break;
                case long longValue:
                    cell.Value = longValue;
                    break;
                case float floatValue:
                    cell.Value = floatValue;
                    break;
                case double doubleValue:
                    cell.Value = doubleValue;
                    break;
                case decimal decimalValue:
                    cell.Value = decimalValue;
                    break;
                case bool boolValue:
                    cell.Value = boolValue;
                    break;
                case DateTime dateTimeValue:
                    cell.Value = dateTimeValue;
                    cell.Style.DateFormat.Format = "dd-MM-yyyy";
                    break;
                case Guid guidValue:
                    cell.Value = guidValue.ToString();
                    break;
                default:
                    cell.Value = value.ToString() ?? string.Empty;
                    break;
            }
        }

        private static bool TryGetPropertyValue(object value, string propertyName, out object? propertyValue)
        {
            var property = value.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                propertyValue = null;
                return false;
            }

            propertyValue = property.GetValue(value);
            return true;
        }

        private static List<string> ToStringList(object? value)
        {
            if (value is not IEnumerable enumerable || value is string)
            {
                return [];
            }

            return enumerable
                .Cast<object?>()
                .Select(item => item?.ToString() ?? string.Empty)
                .ToList();
        }

        private static List<object?> ToObjectList(object? value)
        {
            if (value is not IEnumerable enumerable || value is string)
            {
                return [];
            }

            return enumerable.Cast<object?>().ToList();
        }

        private static string HumanizeLabel(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = Regex.Replace(value, "([A-Z]+)([A-Z][a-z])", "$1 $2");
            normalized = Regex.Replace(normalized, "([a-z0-9])([A-Z])", "$1 $2");
            normalized = normalized.Replace("_", " ").Replace("-", " ");
            normalized = Regex.Replace(normalized, "\\s+", " ").Trim();

            return string.Join(
                ' ',
                normalized
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.Length > 1 && word.All(char.IsUpper)
                        ? word
                        : char.ToUpper(word[0]) + word[1..]));
        }
    }
}
