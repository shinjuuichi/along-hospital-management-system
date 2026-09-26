import './plotly-basic-2.35.2.min.js'

export function initializeChart(id, traces, exemplarTrace, rangeStartTime, rangeEndTime, serverLocale, chartInterop) {
    registerLocale(serverLocale);

    var chartContainerDiv = document.getElementById(id);
    if (!chartContainerDiv) {
        console.log(`Couldn't find container '${id}' when initializing chart.`);
        return;
    }

    // Reusing a div can create issues with chart lines appearing beyond the end range.
    // Workaround this issue by replacing the chart div. Ensures we start from a new state.
    var chartDiv = document.createElement("div");
    chartContainerDiv.replaceChildren(chartDiv);

    var themeColors = getThemeColors();

    var data = [];
    for (var i = 0; i < traces.length; i++) {
        var name = traces[i].name || "Value";
        var t = {
            x: traces[i].x,
            y: traces[i].y,
            name: name,
            text: traces[i].tooltips,
            hoverinfo: 'text',
            stackgroup: "one"
        };
        data.push(t);
    }

    var points = {
        x: exemplarTrace.x,
        y: exemplarTrace.y,
        name: exemplarTrace.name,
        text: exemplarTrace.tooltips,
        hoverinfo: 'text',
        traceData: exemplarTrace.traceData,
        mode: 'markers',
        type: 'scatter',
        marker: {
            size: 16,
            color: themeColors.pointColor,
            line: {
                color: themeColors.backgroundColor,
                width: 1
            }
        }
    };
    data.push(points);

    // Width and height are set using ResizeObserver + ploty resize call.
    var layout = {
        paper_bgcolor: themeColors.backgroundColor,
        plot_bgcolor: themeColors.backgroundColor,
        margin: { t: 0, r: 0, b: 40, l: 50 },
        xaxis: {
            type: 'date',
            range: [rangeEndTime, rangeStartTime],
            fixedrange: true,
            tickformat: "%X",
            color: themeColors.textColor
        },
        yaxis: {
            rangemode: "tozero",
            fixedrange: true,
            color: themeColors.textColor
        },
        hovermode: "closest",
        showlegend: true,
        legend: {
            orientation: "h",
            font: {
                color: themeColors.textColor
            },
            traceorder: "normal",
            itemclick: false,
            itemdoubleclick: false
        }
    };

    var options = { scrollZoom: false, displayModeBar: false };

    var plot = Plotly.newPlot(chartDiv, data, layout, options);

    fixTraceLineRendering(chartDiv);

    // We only want a pointer cursor when the mouse is hovering over an exemplar point.
    // Set the drag layer cursor back to the default and then use plotly_hover/ploty_unhover events to set to pointer.
    var dragLayer = document.getElementsByClassName('nsewdrag')[0];
    dragLayer.style.cursor = 'default';

    // Use mousedown instead of plotly_click event because plotly_click has issues with updating charts.
    // The current point is tracked by setting it with hover/unhover events and then mousedown uses the current value.
    var currentPoint = null;
    chartDiv.on('plotly_hover', function (data) {
        var point = data.points[0];
        if (point.fullData.name == exemplarTrace.name) {
            currentPoint = point;
            dragLayer.style.cursor = 'pointer';
        }
    });
    chartDiv.on('plotly_unhover', function (data) {
        var point = data.points[0];
        if (point.fullData.name == exemplarTrace.name) {
            currentPoint = null;
            dragLayer.style.cursor = 'default';
        }
    });
    chartDiv.addEventListener("mousedown", (e) => {
        if (currentPoint) {
            var point = currentPoint;
            var pointTraceData = point.data.traceData[point.pointIndex];

            chartInterop.invokeMethodAsync('ViewSpan', pointTraceData.traceId, pointTraceData.spanId);
        }
    });

    const resizeObserver = new ResizeObserver(entries => {
        for (let entry of entries) {
            // Don't resize if not visible.
            var display = window.getComputedStyle(entry.target).display;
            var isHidden = !display || display === "none";
            if (!isHidden) {
                Plotly.Plots.resize(entry.target);
            }
        }
    });
    plot.then(plotyDiv => {
        resizeObserver.observe(plotyDiv);
    });
}

export function updateChart(id, traces, exemplarTrace, rangeStartTime, rangeEndTime) {
    var chartContainerDiv = document.getElementById(id);
    if (!chartContainerDiv) {
        console.log(`Couldn't find container '${id}' when updating chart.`);
        return;
    }

    var chartDiv = chartContainerDiv.firstChild;
    if (!chartDiv) {
        console.log(`Couldn't find div inside container '${id}' when updating chart. Chart may not have been successfully initialized.`);
        return;
    }

    var themeColors = getThemeColors();

    var xUpdate = [];
    var yUpdate = [];
    var tooltipsUpdate = [];
    var traceData = [];
    for (var i = 0; i < traces.length; i++) {
        xUpdate.push(traces[i].x);
        yUpdate.push(traces[i].y);
        tooltipsUpdate.push(traces[i].tooltips);
        traceData.push(traces.traceData);
    }

    xUpdate.push(exemplarTrace.x);
    yUpdate.push(exemplarTrace.y);
    tooltipsUpdate.push(exemplarTrace.tooltips);
    traceData.push(exemplarTrace.traceData);

    var data = {
        x: xUpdate,
        y: yUpdate,
        text: tooltipsUpdate,
        traceData: traceData
    };

    var layout = {
        xaxis: {
            type: 'date',
            range: [rangeEndTime, rangeStartTime],
            fixedrange: true,
            tickformat: "%X",
            color: themeColors.textColor
        }
    };

    Plotly.update(chartDiv, data, layout);

    fixTraceLineRendering(chartDiv);
}

function getThemeColors() {
    // Get colors from the current light/dark theme.
    var style = getComputedStyle(document.body);
    return {
        backgroundColor: style.getPropertyValue("--fill-color"),
        textColor: style.getPropertyValue("--neutral-foreground-rest"),
        pointColor: style.getPropertyValue("--accent-fill-rest")
    };
}

function fixTraceLineRendering(chartDiv) {
    // In stack area charts Plotly orders traces so the top line area overwrites the line of areas below it.
    // This isn't the effect we want. When the P50, P90 and P99 values are the same, the line displayed is P99
    // on the P50 area.
    //
    // The fix is to reverse the order of traces so the correct line is on top. There isn't a way to do this
    // with CSS because SVG doesn't support z-index. Node order is what determines the rendering order.
    //
    // https://github.com/plotly/plotly.js/issues/6579
    var parent = chartDiv.querySelector(".scatterlayer");

    if (parent.childNodes.length > 0) {
        for (var i = 1; i < parent.childNodes.length; i++) {
            var child = parent.childNodes[i];
            parent.insertBefore(child, parent.firstChild);
        }

        // Check if there is a trace with points. It should be top most.
        for (var i = 0; i < parent.childNodes.length; i++) {
            var child = parent.childNodes[i];
            if (child.querySelector(".point")) {
                // Append trace to parent to move to the last in the collection.
                parent.appendChild(child);
            }
        }
    }
}

function registerLocale(serverLocale) {
    // Register the locale for Plotly.js. This is to enable localization of time format shown by the charts.
    // Changing plotly.js time formatting is better than supplying values from the server which is very difficult to do correctly.

    // Right now necessary changes are to:
    // -Update AM/PM
    // -Update time format to 12/24 hour.
    var locale = {
        moduleType: 'locale',
        name: 'en',
        dictionary: {
            'Click to enter Colorscale title': 'Click to enter Colourscale title'
        },
        format: {
            days: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
            shortDays: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
            months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            shortMonths: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            periods: serverLocale.periods,
            dateTime: '%a %b %e %X %Y',
            date: '%d/%m/%Y',
            time: serverLocale.time,
            decimal: '.',
            thousands: ',',
            grouping: [3],
            currency: ['$', ''],
            year: '%Y',
            month: '%b %Y',
            dayMonth: '%b %-d',
            dayMonthYear: '%b %-d, %Y'
        }
    };
    Plotly.register(locale);
}

// SIG // Begin signature block
// SIG // MIIoQwYJKoZIhvcNAQcCoIIoNDCCKDACAQExDzANBglg
// SIG // hkgBZQMEAgEFADB3BgorBgEEAYI3AgEEoGkwZzAyBgor
// SIG // BgEEAYI3AgEeMCQCAQEEEBDgyQbOONQRoqMAEEvTUJAC
// SIG // AQACAQACAQACAQACAQAwMTANBglghkgBZQMEAgEFAAQg
// SIG // Gq1C/btibJfKOL8aw1jLXS+Clsy0YW7BgFrg7fNQqi2g
// SIG // gg12MIIF9DCCA9ygAwIBAgITMwAABARsdAb/VysncgAA
// SIG // AAAEBDANBgkqhkiG9w0BAQsFADB+MQswCQYDVQQGEwJV
// SIG // UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
// SIG // UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
// SIG // cmF0aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBT
// SIG // aWduaW5nIFBDQSAyMDExMB4XDTI0MDkxMjIwMTExNFoX
// SIG // DTI1MDkxMTIwMTExNFowdDELMAkGA1UEBhMCVVMxEzAR
// SIG // BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
// SIG // bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
// SIG // bjEeMBwGA1UEAxMVTWljcm9zb2Z0IENvcnBvcmF0aW9u
// SIG // MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA
// SIG // tCg32mOdDA6rBBnZSMwxwXegqiDEUFlvQH9Sxww07hY3
// SIG // w7L52tJxLg0mCZjcszQddI6W4NJYb5E9QM319kyyE0l8
// SIG // EvA/pgcxgljDP8E6XIlgVf6W40ms286Cr0azaA1f7vaJ
// SIG // jjNhGsMqOSSSXTZDNnfKs5ENG0bkXeB2q5hrp0qLsm/T
// SIG // WO3oFjeROZVHN2tgETswHR3WKTm6QjnXgGNj+V6rSZJO
// SIG // /WkTqc8NesAo3Up/KjMwgc0e67x9llZLxRyyMWUBE9co
// SIG // T2+pUZqYAUDZ84nR1djnMY3PMDYiA84Gw5JpceeED38O
// SIG // 0cEIvKdX8uG8oQa047+evMfDRr94MG9EWwIDAQABo4IB
// SIG // czCCAW8wHwYDVR0lBBgwFgYKKwYBBAGCN0wIAQYIKwYB
// SIG // BQUHAwMwHQYDVR0OBBYEFPIboTWxEw1PmVpZS+AzTDwo
// SIG // oxFOMEUGA1UdEQQ+MDykOjA4MR4wHAYDVQQLExVNaWNy
// SIG // b3NvZnQgQ29ycG9yYXRpb24xFjAUBgNVBAUTDTIzMDAx
// SIG // Mis1MDI5MjMwHwYDVR0jBBgwFoAUSG5k5VAF04KqFzc3
// SIG // IrVtqMp1ApUwVAYDVR0fBE0wSzBJoEegRYZDaHR0cDov
// SIG // L3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9jcmwvTWlj
// SIG // Q29kU2lnUENBMjAxMV8yMDExLTA3LTA4LmNybDBhBggr
// SIG // BgEFBQcBAQRVMFMwUQYIKwYBBQUHMAKGRWh0dHA6Ly93
// SIG // d3cubWljcm9zb2Z0LmNvbS9wa2lvcHMvY2VydHMvTWlj
// SIG // Q29kU2lnUENBMjAxMV8yMDExLTA3LTA4LmNydDAMBgNV
// SIG // HRMBAf8EAjAAMA0GCSqGSIb3DQEBCwUAA4ICAQCI5g/S
// SIG // KUFb3wdUHob6Qhnu0Hk0JCkO4925gzI8EqhS+K4umnvS
// SIG // BU3acsJ+bJprUiMimA59/5x7WhJ9F9TQYy+aD9AYwMtb
// SIG // KsQ/rst+QflfML+Rq8YTAyT/JdkIy7R/1IJUkyIS6srf
// SIG // G1AKlX8n6YeAjjEb8MI07wobQp1F1wArgl2B1mpTqHND
// SIG // lNqBjfpjySCScWjUHNbIwbDGxiFr93JoEh5AhJqzL+8m
// SIG // onaXj7elfsjzIpPnl8NyH2eXjTojYC9a2c4EiX0571Ko
// SIG // mhENF3RtR25A7/X7+gk6upuE8tyMy4sBkl2MUSF08U+E
// SIG // 2LOVcR8trhYxV1lUi9CdgEU2CxODspdcFwxdT1+G8YNc
// SIG // gzHyjx3BNSI4nOZcdSnStUpGhCXbaOIXfvtOSfQX/UwJ
// SIG // oruhCugvTnub0Wna6CQiturglCOMyIy/6hu5rMFvqk9A
// SIG // ltIJ0fSR5FwljW6PHHDJNbCWrZkaEgIn24M2mG1M/Ppb
// SIG // /iF8uRhbgJi5zWxo2nAdyDBqWvpWxYIoee/3yIWpquVY
// SIG // cYGhJp/1I1sq/nD4gBVrk1SKX7Do2xAMMO+cFETTNSJq
// SIG // fTSSsntTtuBLKRB5mw5qglHKuzapDiiBuD1Zt4QwxA/1
// SIG // kKcyQ5L7uBayG78kxlVNNbyrIOFH3HYmdH0Pv1dIX/Mq
// SIG // 7avQpAfIiLpOWwcbjzCCB3owggVioAMCAQICCmEOkNIA
// SIG // AAAAAAMwDQYJKoZIhvcNAQELBQAwgYgxCzAJBgNVBAYT
// SIG // AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
// SIG // EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
// SIG // cG9yYXRpb24xMjAwBgNVBAMTKU1pY3Jvc29mdCBSb290
// SIG // IENlcnRpZmljYXRlIEF1dGhvcml0eSAyMDExMB4XDTEx
// SIG // MDcwODIwNTkwOVoXDTI2MDcwODIxMDkwOVowfjELMAkG
// SIG // A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAO
// SIG // BgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29m
// SIG // dCBDb3Jwb3JhdGlvbjEoMCYGA1UEAxMfTWljcm9zb2Z0
// SIG // IENvZGUgU2lnbmluZyBQQ0EgMjAxMTCCAiIwDQYJKoZI
// SIG // hvcNAQEBBQADggIPADCCAgoCggIBAKvw+nIQHC6t2G6q
// SIG // ghBNNLrytlghn0IbKmvpWlCquAY4GgRJun/DDB7dN2vG
// SIG // EtgL8DjCmQawyDnVARQxQtOJDXlkh36UYCRsr55JnOlo
// SIG // XtLfm1OyCizDr9mpK656Ca/XllnKYBoF6WZ26DJSJhIv
// SIG // 56sIUM+zRLdd2MQuA3WraPPLbfM6XKEW9Ea64DhkrG5k
// SIG // NXimoGMPLdNAk/jj3gcN1Vx5pUkp5w2+oBN3vpQ97/vj
// SIG // K1oQH01WKKJ6cuASOrdJXtjt7UORg9l7snuGG9k+sYxd
// SIG // 6IlPhBryoS9Z5JA7La4zWMW3Pv4y07MDPbGyr5I4ftKd
// SIG // gCz1TlaRITUlwzluZH9TupwPrRkjhMv0ugOGjfdf8NBS
// SIG // v4yUh7zAIXQlXxgotswnKDglmDlKNs98sZKuHCOnqWbs
// SIG // YR9q4ShJnV+I4iVd0yFLPlLEtVc/JAPw0XpbL9Uj43Bd
// SIG // D1FGd7P4AOG8rAKCX9vAFbO9G9RVS+c5oQ/pI0m8GLhE
// SIG // fEXkwcNyeuBy5yTfv0aZxe/CHFfbg43sTUkwp6uO3+xb
// SIG // n6/83bBm4sGXgXvt1u1L50kppxMopqd9Z4DmimJ4X7Iv
// SIG // hNdXnFy/dygo8e1twyiPLI9AN0/B4YVEicQJTMXUpUMv
// SIG // dJX3bvh4IFgsE11glZo+TzOE2rCIF96eTvSWsLxGoGyY
// SIG // 0uDWiIwLAgMBAAGjggHtMIIB6TAQBgkrBgEEAYI3FQEE
// SIG // AwIBADAdBgNVHQ4EFgQUSG5k5VAF04KqFzc3IrVtqMp1
// SIG // ApUwGQYJKwYBBAGCNxQCBAweCgBTAHUAYgBDAEEwCwYD
// SIG // VR0PBAQDAgGGMA8GA1UdEwEB/wQFMAMBAf8wHwYDVR0j
// SIG // BBgwFoAUci06AjGQQ7kUBU7h6qfHMdEjiTQwWgYDVR0f
// SIG // BFMwUTBPoE2gS4ZJaHR0cDovL2NybC5taWNyb3NvZnQu
// SIG // Y29tL3BraS9jcmwvcHJvZHVjdHMvTWljUm9vQ2VyQXV0
// SIG // MjAxMV8yMDExXzAzXzIyLmNybDBeBggrBgEFBQcBAQRS
// SIG // MFAwTgYIKwYBBQUHMAKGQmh0dHA6Ly93d3cubWljcm9z
// SIG // b2Z0LmNvbS9wa2kvY2VydHMvTWljUm9vQ2VyQXV0MjAx
// SIG // MV8yMDExXzAzXzIyLmNydDCBnwYDVR0gBIGXMIGUMIGR
// SIG // BgkrBgEEAYI3LgMwgYMwPwYIKwYBBQUHAgEWM2h0dHA6
// SIG // Ly93d3cubWljcm9zb2Z0LmNvbS9wa2lvcHMvZG9jcy9w
// SIG // cmltYXJ5Y3BzLmh0bTBABggrBgEFBQcCAjA0HjIgHQBM
// SIG // AGUAZwBhAGwAXwBwAG8AbABpAGMAeQBfAHMAdABhAHQA
// SIG // ZQBtAGUAbgB0AC4gHTANBgkqhkiG9w0BAQsFAAOCAgEA
// SIG // Z/KGpZjgVHkaLtPYdGcimwuWEeFjkplCln3SeQyQwWVf
// SIG // Liw++MNy0W2D/r4/6ArKO79HqaPzadtjvyI1pZddZYSQ
// SIG // fYtGUFXYDJJ80hpLHPM8QotS0LD9a+M+By4pm+Y9G6XU
// SIG // tR13lDni6WTJRD14eiPzE32mkHSDjfTLJgJGKsKKELuk
// SIG // qQUMm+1o+mgulaAqPyprWEljHwlpblqYluSD9MCP80Yr
// SIG // 3vw70L01724lruWvJ+3Q3fMOr5kol5hNDj0L8giJ1h/D
// SIG // Mhji8MUtzluetEk5CsYKwsatruWy2dsViFFFWDgycSca
// SIG // f7H0J/jeLDogaZiyWYlobm+nt3TDQAUGpgEqKD6CPxNN
// SIG // ZgvAs0314Y9/HG8VfUWnduVAKmWjw11SYobDHWM2l4bf
// SIG // 2vP48hahmifhzaWX0O5dY0HjWwechz4GdwbRBrF1HxS+
// SIG // YWG18NzGGwS+30HHDiju3mUv7Jf2oVyW2ADWoUa9WfOX
// SIG // pQlLSBCZgB/QACnFsZulP0V3HjXG0qKin3p6IvpIlR+r
// SIG // +0cjgPWe+L9rt0uX4ut1eBrs6jeZeRhL/9azI2h15q/6
// SIG // /IvrC4DqaTuv/DDtBEyO3991bWORPdGdVk5Pv4BXIqF4
// SIG // ETIheu9BCrE/+6jMpF3BoYibV3FWTkhFwELJm3ZbCoBI
// SIG // a/15n8G9bW1qyVJzEw16UM0xgholMIIaIQIBATCBlTB+
// SIG // MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3Rv
// SIG // bjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWlj
// SIG // cm9zb2Z0IENvcnBvcmF0aW9uMSgwJgYDVQQDEx9NaWNy
// SIG // b3NvZnQgQ29kZSBTaWduaW5nIFBDQSAyMDExAhMzAAAE
// SIG // BGx0Bv9XKydyAAAAAAQEMA0GCWCGSAFlAwQCAQUAoIGu
// SIG // MBkGCSqGSIb3DQEJAzEMBgorBgEEAYI3AgEEMBwGCisG
// SIG // AQQBgjcCAQsxDjAMBgorBgEEAYI3AgEVMC8GCSqGSIb3
// SIG // DQEJBDEiBCCN0PAwtolw/Q8mhHNMtFp0MWNT0EugD2Sm
// SIG // US5yPu9HpjBCBgorBgEEAYI3AgEMMTQwMqAUgBIATQBp
// SIG // AGMAcgBvAHMAbwBmAHShGoAYaHR0cDovL3d3dy5taWNy
// SIG // b3NvZnQuY29tMA0GCSqGSIb3DQEBAQUABIIBAHPyohYw
// SIG // fE4atrt286ZNuWMy9cUnLOmsl+te0HhVEiWoUKq14890
// SIG // 2Qx4jIeluk2nerbzOgRkrDMfJF19Nou2nCiTCiNJOfH9
// SIG // vkSqzS+QHLOM8XpsOO8KWh2oyMD7TZO3eeMizKu2Jg9a
// SIG // 9H6hkMTfD4lqo73DMFfIfSVnFYQq+f5UtQCfDQTdGzFW
// SIG // +IFOra2xbi01h8XmNNUfO8s5Qu7Bf9yKHaYqe/H3R/QO
// SIG // RbYr8fEaDlFPEYr6NEO5+huqeiCqf/S0l5ajHe/XfmqU
// SIG // 2otJVVvSDx9Nbq0hBF4gSQJZhJAVrSYxizPW4EOBBB52
// SIG // RZf/Uqo/qqw8tz4tugMrbzuTCJGhghevMIIXqwYKKwYB
// SIG // BAGCNwMDATGCF5swgheXBgkqhkiG9w0BBwKggheIMIIX
// SIG // hAIBAzEPMA0GCWCGSAFlAwQCAQUAMIIBWQYLKoZIhvcN
// SIG // AQkQAQSgggFIBIIBRDCCAUACAQEGCisGAQQBhFkKAwEw
// SIG // MTANBglghkgBZQMEAgEFAAQgcyDKn/syF+jTH7lrxmxM
// SIG // NmL4GNiYqbQlu2yKou0senwCBmguS8T3ixgSMjAyNTA2
// SIG // MDYxNDI0MzguNDhaMASAAgH0oIHZpIHWMIHTMQswCQYD
// SIG // VQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
// SIG // A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
// SIG // IENvcnBvcmF0aW9uMS0wKwYDVQQLEyRNaWNyb3NvZnQg
// SIG // SXJlbGFuZCBPcGVyYXRpb25zIExpbWl0ZWQxJzAlBgNV
// SIG // BAsTHm5TaGllbGQgVFNTIEVTTjo1NTFBLTA1RTAtRDk0
// SIG // NzElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAg
// SIG // U2VydmljZaCCEf4wggcoMIIFEKADAgECAhMzAAACAdFF
// SIG // WZgQzEJPAAEAAAIBMA0GCSqGSIb3DQEBCwUAMHwxCzAJ
// SIG // BgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAw
// SIG // DgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3Nv
// SIG // ZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29m
// SIG // dCBUaW1lLVN0YW1wIFBDQSAyMDEwMB4XDTI0MDcyNTE4
// SIG // MzEyMloXDTI1MTAyMjE4MzEyMlowgdMxCzAJBgNVBAYT
// SIG // AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
// SIG // EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
// SIG // cG9yYXRpb24xLTArBgNVBAsTJE1pY3Jvc29mdCBJcmVs
// SIG // YW5kIE9wZXJhdGlvbnMgTGltaXRlZDEnMCUGA1UECxMe
// SIG // blNoaWVsZCBUU1MgRVNOOjU1MUEtMDVFMC1EOTQ3MSUw
// SIG // IwYDVQQDExxNaWNyb3NvZnQgVGltZS1TdGFtcCBTZXJ2
// SIG // aWNlMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKC
// SIG // AgEAtWrf+HzDu7sk50y5YHheCIJG0uxRSFFcHNek+Td9
// SIG // ZmyJj20EEjaU8JDJu5pWc4pPAsBI38NEAJ1b+KBnlStq
// SIG // U8uvXF4qnEShDdi8nPsZZQsTZDKWAgUM2iZTOiWIuZcF
// SIG // s5ZC8/+GlrVLM5h1Y9nfMh5B4DnUQOXMremAT9MkvUhg
// SIG // 3uaYgmqLlmYyODmba4lXZBu104SLAFsXOfl/TLhpToT4
// SIG // 6y7lI9sbI9uq3/Aerh3aPi2knHvEEazilXeooXNLCwdu
// SIG // +Is6o8kQLouUn3KwUQm0b7aUtsv1X/OgPmsOJi6yN3LY
// SIG // WyHISvrNuIrJ4iYNgHdBBumQYK8LjZmQaTKFacxhmXJ0
// SIG // q2gzaIfxF2yIwM+V9sQqkHkg/Q+iSDNpMr6mr/OwknOE
// SIG // IjI0g6ZMOymivpChzDNoPz9hkK3gVHZKW7NV8+UBXN4G
// SIG // 0aBX69fKUbxBBLyk2cC+PhOoUjkl6UC8/c0huqj5xX8m
// SIG // +YVIk81e7t6I+V/E4yXReeZgr0FhYqNpvTjGcaO2WrkP
// SIG // 5XmsYS7IvMPIf4DCyIJUZaqoBMToAJJHGRe+DPqCHg6b
// SIG // mGPm97MrOWv16/Co6S9cQDkXp9vMSSRQWXy4KtJhZfmu
// SIG // Dz2vr1jw4NeixwuIDGw1mtV/TdSI+vpLJfUiLl/b9w/t
// SIG // JB92BALQT8e1YH8NphdOo1xCwkcCAwEAAaOCAUkwggFF
// SIG // MB0GA1UdDgQWBBSwcq9blqLoPPiVrym9mFmFWbyyUjAf
// SIG // BgNVHSMEGDAWgBSfpxVdAF5iXYP05dJlpxtTNRnpcjBf
// SIG // BgNVHR8EWDBWMFSgUqBQhk5odHRwOi8vd3d3Lm1pY3Jv
// SIG // c29mdC5jb20vcGtpb3BzL2NybC9NaWNyb3NvZnQlMjBU
// SIG // aW1lLVN0YW1wJTIwUENBJTIwMjAxMCgxKS5jcmwwbAYI
// SIG // KwYBBQUHAQEEYDBeMFwGCCsGAQUFBzAChlBodHRwOi8v
// SIG // d3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2NlcnRzL01p
// SIG // Y3Jvc29mdCUyMFRpbWUtU3RhbXAlMjBQQ0ElMjAyMDEw
// SIG // KDEpLmNydDAMBgNVHRMBAf8EAjAAMBYGA1UdJQEB/wQM
// SIG // MAoGCCsGAQUFBwMIMA4GA1UdDwEB/wQEAwIHgDANBgkq
// SIG // hkiG9w0BAQsFAAOCAgEAOjQAyz0cVztTFGqXX5JLRxFK
// SIG // /O/oMe55uDqEC8Vd1gbcM28KBUPgvUIPXm/vdDN2IVBk
// SIG // WHmwCp4AIcy4dZtkuUmd0fnu6aT9Mvo1ndsLp2YJcMoF
// SIG // LEt3TtriLaO+i4Grv0ZULtWXUPAW/Mn5Scjgn0xZduGP
// SIG // BD/Xs3J7+get9+8ZvBipsg/N7poimYOVsHxLcem7V5Xd
// SIG // MNsytTm/uComhM/wgR5KlDYTVNAXBxcSKMeJaiD3V1+H
// SIG // hNkVliMl5VOP+nw5xWF55u9h6eF2G7eBPqT+qSFQ+rQC
// SIG // QdIrN0yG1QN9PJroguK+FJQJdQzdfD3RWVsciBygbYaZ
// SIG // lT1cGJI1IyQ74DQ0UBdTpfeGsyrEQ9PI8QyqVLqb2q7L
// SIG // tI6DJMNphYu+jr//0spr1UVvyDPtuRnbGQRNi1COwJcj
// SIG // 9OYmlkFgKNeCfbDT7U3uEOvWomekX60Y/m5utRcUPVeA
// SIG // PdhkB+DxDaev3J1ywDNdyu911nAVPgRkyKgMK3USLG37
// SIG // EdlatDk8FyuCrx4tiHyqHO3wE6xPw32Q8e/vmuQPoBZu
// SIG // X3qUeoFIsyZEenHq2ScMunhcqW32SUVAi5oZ4Z3nf7dA
// SIG // gNau21NEPwgW+2wkrNqDg7Hp8yHyoOKbgEBu6REQbvSf
// SIG // Z5Kh4PV+S2gxf2uq6GoYDnlqABOMYwz309ISi0bPMh8w
// SIG // ggdxMIIFWaADAgECAhMzAAAAFcXna54Cm0mZAAAAAAAV
// SIG // MA0GCSqGSIb3DQEBCwUAMIGIMQswCQYDVQQGEwJVUzET
// SIG // MBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVk
// SIG // bW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0
// SIG // aW9uMTIwMAYDVQQDEylNaWNyb3NvZnQgUm9vdCBDZXJ0
// SIG // aWZpY2F0ZSBBdXRob3JpdHkgMjAxMDAeFw0yMTA5MzAx
// SIG // ODIyMjVaFw0zMDA5MzAxODMyMjVaMHwxCzAJBgNVBAYT
// SIG // AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
// SIG // EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
// SIG // cG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1l
// SIG // LVN0YW1wIFBDQSAyMDEwMIICIjANBgkqhkiG9w0BAQEF
// SIG // AAOCAg8AMIICCgKCAgEA5OGmTOe0ciELeaLL1yR5vQ7V
// SIG // gtP97pwHB9KpbE51yMo1V/YBf2xK4OK9uT4XYDP/XE/H
// SIG // ZveVU3Fa4n5KWv64NmeFRiMMtY0Tz3cywBAY6GB9alKD
// SIG // RLemjkZrBxTzxXb1hlDcwUTIcVxRMTegCjhuje3XD9gm
// SIG // U3w5YQJ6xKr9cmmvHaus9ja+NSZk2pg7uhp7M62AW36M
// SIG // EBydUv626GIl3GoPz130/o5Tz9bshVZN7928jaTjkY+y
// SIG // OSxRnOlwaQ3KNi1wjjHINSi947SHJMPgyY9+tVSP3PoF
// SIG // VZhtaDuaRr3tpK56KTesy+uDRedGbsoy1cCGMFxPLOJi
// SIG // ss254o2I5JasAUq7vnGpF1tnYN74kpEeHT39IM9zfUGa
// SIG // RnXNxF803RKJ1v2lIH1+/NmeRd+2ci/bfV+Autuqfjbs
// SIG // Nkz2K26oElHovwUDo9Fzpk03dJQcNIIP8BDyt0cY7afo
// SIG // mXw/TNuvXsLz1dhzPUNOwTM5TI4CvEJoLhDqhFFG4tG9
// SIG // ahhaYQFzymeiXtcodgLiMxhy16cg8ML6EgrXY28MyTZk
// SIG // i1ugpoMhXV8wdJGUlNi5UPkLiWHzNgY1GIRH29wb0f2y
// SIG // 1BzFa/ZcUlFdEtsluq9QBXpsxREdcu+N+VLEhReTwDwV
// SIG // 2xo3xwgVGD94q0W29R6HXtqPnhZyacaue7e3PmriLq0C
// SIG // AwEAAaOCAd0wggHZMBIGCSsGAQQBgjcVAQQFAgMBAAEw
// SIG // IwYJKwYBBAGCNxUCBBYEFCqnUv5kxJq+gpE8RjUpzxD/
// SIG // LwTuMB0GA1UdDgQWBBSfpxVdAF5iXYP05dJlpxtTNRnp
// SIG // cjBcBgNVHSAEVTBTMFEGDCsGAQQBgjdMg30BATBBMD8G
// SIG // CCsGAQUFBwIBFjNodHRwOi8vd3d3Lm1pY3Jvc29mdC5j
// SIG // b20vcGtpb3BzL0RvY3MvUmVwb3NpdG9yeS5odG0wEwYD
// SIG // VR0lBAwwCgYIKwYBBQUHAwgwGQYJKwYBBAGCNxQCBAwe
// SIG // CgBTAHUAYgBDAEEwCwYDVR0PBAQDAgGGMA8GA1UdEwEB
// SIG // /wQFMAMBAf8wHwYDVR0jBBgwFoAU1fZWy4/oolxiaNE9
// SIG // lJBb186aGMQwVgYDVR0fBE8wTTBLoEmgR4ZFaHR0cDov
// SIG // L2NybC5taWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVj
// SIG // dHMvTWljUm9vQ2VyQXV0XzIwMTAtMDYtMjMuY3JsMFoG
// SIG // CCsGAQUFBwEBBE4wTDBKBggrBgEFBQcwAoY+aHR0cDov
// SIG // L3d3dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNS
// SIG // b29DZXJBdXRfMjAxMC0wNi0yMy5jcnQwDQYJKoZIhvcN
// SIG // AQELBQADggIBAJ1VffwqreEsH2cBMSRb4Z5yS/ypb+pc
// SIG // FLY+TkdkeLEGk5c9MTO1OdfCcTY/2mRsfNB1OW27DzHk
// SIG // wo/7bNGhlBgi7ulmZzpTTd2YurYeeNg2LpypglYAA7AF
// SIG // vonoaeC6Ce5732pvvinLbtg/SHUB2RjebYIM9W0jVOR4
// SIG // U3UkV7ndn/OOPcbzaN9l9qRWqveVtihVJ9AkvUCgvxm2
// SIG // EhIRXT0n4ECWOKz3+SmJw7wXsFSFQrP8DJ6LGYnn8Atq
// SIG // gcKBGUIZUnWKNsIdw2FzLixre24/LAl4FOmRsqlb30mj
// SIG // dAy87JGA0j3mSj5mO0+7hvoyGtmW9I/2kQH2zsZ0/fZM
// SIG // cm8Qq3UwxTSwethQ/gpY3UA8x1RtnWN0SCyxTkctwRQE
// SIG // cb9k+SS+c23Kjgm9swFXSVRk2XPXfx5bRAGOWhmRaw2f
// SIG // pCjcZxkoJLo4S5pu+yFUa2pFEUep8beuyOiJXk+d0tBM
// SIG // drVXVAmxaQFEfnyhYWxz/gq77EFmPWn9y8FBSX5+k77L
// SIG // +DvktxW/tM4+pTFRhLy/AsGConsXHRWJjXD+57XQKBqJ
// SIG // C4822rpM+Zv/Cuk0+CQ1ZyvgDbjmjJnW4SLq8CdCPSWU
// SIG // 5nR0W2rRnj7tfqAxM328y+l7vzhwRNGQ8cirOoo6CGJ/
// SIG // 2XBjU02N7oJtpQUQwXEGahC0HVUzWLOhcGbyoYIDWTCC
// SIG // AkECAQEwggEBoYHZpIHWMIHTMQswCQYDVQQGEwJVUzET
// SIG // MBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVk
// SIG // bW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0
// SIG // aW9uMS0wKwYDVQQLEyRNaWNyb3NvZnQgSXJlbGFuZCBP
// SIG // cGVyYXRpb25zIExpbWl0ZWQxJzAlBgNVBAsTHm5TaGll
// SIG // bGQgVFNTIEVTTjo1NTFBLTA1RTAtRDk0NzElMCMGA1UE
// SIG // AxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAgU2VydmljZaIj
// SIG // CgEBMAcGBSsOAwIaAxUA1+26cR/yH100DiNFGWhuAv2r
// SIG // YBqggYMwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UE
// SIG // CBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEe
// SIG // MBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYw
// SIG // JAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Eg
// SIG // MjAxMDANBgkqhkiG9w0BAQsFAAIFAOvtN/YwIhgPMjAy
// SIG // NTA2MDYwOTQ4MzhaGA8yMDI1MDYwNzA5NDgzOFowdzA9
// SIG // BgorBgEEAYRZCgQBMS8wLTAKAgUA6+039gIBADAKAgEA
// SIG // AgISMAIB/zAHAgEAAgITCDAKAgUA6+6JdgIBADA2Bgor
// SIG // BgEEAYRZCgQCMSgwJjAMBgorBgEEAYRZCgMCoAowCAIB
// SIG // AAIDB6EgoQowCAIBAAIDAYagMA0GCSqGSIb3DQEBCwUA
// SIG // A4IBAQASkSfuRd4lYVRE5n+UWDs8fFYFuF21IwIFRty9
// SIG // ivTOxBkGLHNUl4QXktjaR9lsCCQbBHmFsxRANpX+AxcX
// SIG // vdpZ9VXeRcC8Y4UbzH2ir7tx4x08EMc/wTd3Il7u5J0X
// SIG // mCO3S3UGFGHPYGDM+ICmpFqxJPmgO/yA9YE5A/zWKARp
// SIG // Br7jpzWhdU1mXMnQ1cczrgf89KmRjzn41ZygjhGsNI1m
// SIG // ZR68RRGUmJ+k9lkYiVcJ08pKocpDEysEe1XpAQDguFOy
// SIG // ua7+lhH8xB72+SrwsM0ayMGn1teQDZ4ARCeu0I0WHOUW
// SIG // DIe9WmzoALuXQw7rPM8CvZqFN2bN/i0T+ucqzOKdMYIE
// SIG // DTCCBAkCAQEwgZMwfDELMAkGA1UEBhMCVVMxEzARBgNV
// SIG // BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
// SIG // HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEm
// SIG // MCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENB
// SIG // IDIwMTACEzMAAAIB0UVZmBDMQk8AAQAAAgEwDQYJYIZI
// SIG // AWUDBAIBBQCgggFKMBoGCSqGSIb3DQEJAzENBgsqhkiG
// SIG // 9w0BCRABBDAvBgkqhkiG9w0BCQQxIgQg7FpqHF+xvidC
// SIG // JSBhSFK7LZM+76sjnT2DNtJYem1mx6gwgfoGCyqGSIb3
// SIG // DQEJEAIvMYHqMIHnMIHkMIG9BCBYa7I6TJQRcmx0HaST
// SIG // WZdJgowdrl9+Zrr0pIdqHtc4IzCBmDCBgKR+MHwxCzAJ
// SIG // BgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAw
// SIG // DgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3Nv
// SIG // ZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29m
// SIG // dCBUaW1lLVN0YW1wIFBDQSAyMDEwAhMzAAACAdFFWZgQ
// SIG // zEJPAAEAAAIBMCIEIIlji/V9K6XYRm+z0SmTLj/cy/5N
// SIG // HvYD5HvPycvCcUkBMA0GCSqGSIb3DQEBCwUABIICAIZt
// SIG // oIhwjXiN26m+OeNTcq52j8sboF/uFdoiPhOE8PsVqThx
// SIG // R31ErI4TkLenBFvMFaAdSJH3li//27Rd/z0bdnq2WUFo
// SIG // NBKgBvXzadIcGXdvPTzvbPCS/IZvbhnRzC7Um6yhpaou
// SIG // NZ1v/T0QSUmuLWpw0D3KxLI0i9hk1SnYJgTMgr+az6nB
// SIG // sbxcCAEXjE33e1XZfjHSWIIlXQU0sd+Q36FtXhMk8h3G
// SIG // sZ4eMtpZGZgcqbPWZmSZiu5fPAW4a5XDxIdQoLbgVSr4
// SIG // cxgmuIYhb+XiGQFov0i/FOEhNcFtA3IZ41qnEa5VY86y
// SIG // RikWiwjP2f+0v+s4ydQ03k3BgBKAIrQJlpSQEPSSVzcr
// SIG // gn73yrVdacwRTlXzPwcU/f22OqzyG8CyTQYjloRgFaOz
// SIG // +EGyEJSghIN5qNs/f4oU8FvGthU9asH3t2REkI58pgX/
// SIG // zXokZ0rs5Nuq+b22Sw3TSYsJexdigXKKs8Bx0TerjxXP
// SIG // tlaLUFLJIzOBi2ZEwLGd05GTZUaSfGmB40Wa/w8vYpDY
// SIG // MIGFk7tcLoTszjkSzlc52YV4C7XiU2Mx9aYS6OpFm4PK
// SIG // mANogY3XhkvwHe5T1Zbbvu36AuCE8wxFvDQs6vN5EkEw
// SIG // AIsqqeGZv3D5UWxlERf+903aYf7ikMOJ5xc9pFGb8yzr
// SIG // 2lmQDZQNK1E6xcFXBQvY
// SIG // End signature block
