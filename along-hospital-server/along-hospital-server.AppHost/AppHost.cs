#pragma warning disable ASPIREHOSTINGPYTHON001
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//API Gateway
builder.AddProject<Projects.ApiGateway>("apigateway");

//Common services
builder.AddProject<Projects.EmailSvc>("commons-emailsvc");
builder.AddProject<Projects.SmsSvc>("commons-smssvc");
builder.AddProject<Projects.UploadSvc>("commons-uploadsvc");
var mlSvc = builder.AddPythonApp(
    "commons-mlsvc",
    "../CommonServices/MachineLearningSvc",
    "main.py",
    virtualEnvironmentPath: ".venv")
        .WithEnvironment("PYTHONPATH", Path.GetFullPath(".."))
        .WithHttpEndpoint(port: 4996, targetPort: 7353)
        .WithExternalHttpEndpoints()
        .WithOtlpExporter();

//Example Service
//builder.AddProject<Projects.ProductSvc_WebAPI>("productsvc-webapi");

//Services
builder.AddProject<Projects.AuthSvc_WebAPI>("authsvc-webapi");

builder.AddProject<Projects.UserSvc_WebAPI>("usersvc-webapi");

builder.AddProject<Projects.StaffSvc_WebAPI>("staffsvc-webapi");

builder.AddProject<Projects.PatientSvc_WebAPI>("patientsvc-webapi");

builder.AddProject<Projects.BlogSvc_WebAPI>("blogsvc-webapi");

builder.AddProject<Projects.PaymentSvc_WebAPI>("paymentsvc-webapi");

builder.AddProject<Projects.AppointmentSvc_WebAPI>("appointmentsvc-webapi");

builder.AddProject<Projects.InventorySvc_WebAPI>("inventory-webapi");

builder.AddProject<Projects.MedicalHistorySvc_WebAPI>("medicalhistorysvc-webapi");

builder.AddProject<Projects.FeedbackSvc_WebAPI>("feedback-webapi");

builder.AddProject<Projects.MedicineSvc_WebAPI>("medicinesvc-webapi");

builder.AddProject<Projects.MedicalServiceSvc_WebAPI>("medicalservicesvc-webapi");

builder.AddProject<Projects.OrderSvc_WebAPI>("ordersvc-webapi");

builder.AddProject<Projects.CartSvc_WebAPI>("cartsvc-webapi");

builder.AddProject<Projects.TeleHealthSvc_WebAPI>("telehealthsvc-webapi");

builder.AddProject<Projects.SupplierSvc_WebAPI>("suppliersvc-webapi");

builder.AddProject<Projects.VoucherSvc_WebAPI>("vouchersvc-webapi");

builder.AddProject<Projects.ChatboxSvc_WebAPI>("chatboxsvc-webapi");

builder.AddProject<Projects.AttendanceSvc_WebAPI>("attendancesvc-webapi");

builder.AddProject<Projects.BillingSvc_WebAPI>("billingsvc-webapi");

builder.AddProject<Projects.InpatientResourceSvc_WebAPI>("inpatientresourcesvc-webapi");

builder.AddProject<Projects.WorkScheduleSvc_WebAPI>("workschedulesvc-webapi");

builder.AddProject<Projects.PayrollSvc_WebAPI>("payrollsvc-webapi");

builder.AddProject<Projects.RecruitmentSvc_WebAPI>("recruitmentsvc-webapi");

builder.AddProject<Projects.StaffRequestSvc_WebAPI>("staffrequestsvc-webapi");

builder.AddProject<Projects.MedicalOrderSvc_WebAPI>("medicalordersvc-webapi");

builder.AddProject<Projects.ReportSvc_WebAPI>("reportsvc-webapi");

builder.AddProject<Projects.QueueSvc_WebAPI>("queuesvc-webapi");

builder.Build().Run();

if (builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
{
    mlSvc.WithEnvironment("DEBUG", "True");
}

builder.Build().Run();