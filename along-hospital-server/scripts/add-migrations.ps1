# Add Migrations Script for Along Hospital Server
# Usage: .\add-migrations.ps1                                    # Add for ALL services
#        .\add-migrations.ps1 -Services @("Product", "Auth")     # Add for specific services
#        .\add-migrations.ps1 -MigrationName "AddNewColumn"      # Add for ALL with custom name
#
# Path auto-generated from service name following pattern:
#   WebAPI: {ServiceName}Service\{ServiceName}Svc.WebAPI
#   DAL:    {ServiceName}Service\{ServiceName}Svc.DAL

param(
    [Parameter(Mandatory = $false)]
    [string[]]$Services = @(),
    
    [Parameter(Mandatory = $false)]
    [string]$MigrationName = "Init"
)

$ErrorActionPreference = "Stop"
$RootPath = Split-Path -Parent $PSScriptRoot

# All available service names
$AllServices = @(
    "Auth",
    "User",
    "Patient",
    "Staff",
    "Appointment",
    "Attendance",
    "Blog",
    "Cart",
    "Feedback",
    "Inventory",
    "MedicalHistory",
    "MedicalService",
    "Medicine",
    "Order",
    "Product",
    "Supplier",
    "InpatientResource",
    "Billing",
    "TeleHealth",
    "WorkSchedule"
)

# Determine target services (default: ALL)
if ($Services.Count -gt 0) {
    $TargetServices = $Services
} else {
    $TargetServices = $AllServices
}

# Override paths for services that don't follow standard pattern
# Format: ServiceName = @{ WebAPI = "..."; DAL = "..." }
$PathOverrides = @{
    # "MedicalService" = @{ WebAPI = "MedicalService\MedicalServiceSvc.WebAPI"; DAL = "MedicalService\MedicalServiceSvc.DAL" }
}

# Function to get service paths (auto-generate or use override)
function Get-ServicePaths($serviceName) {
    if ($PathOverrides.ContainsKey($serviceName)) {
        return $PathOverrides[$serviceName]
    }
    # Auto-generate path from service name
    return @{
        WebAPI = "${serviceName}Service\${serviceName}Svc.WebAPI"
        DAL    = "${serviceName}Service\${serviceName}Svc.DAL"
    }
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Add Migrations: $MigrationName" -ForegroundColor Cyan
Write-Host "  Services: $(if ($Services.Count -eq 0) { 'ALL' } else { $TargetServices -join ', ' })" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$SuccessCount = 0
$FailCount = 0
$Results = @()

foreach ($serviceName in $TargetServices) {
    $paths = Get-ServicePaths $serviceName
    $webApiPath = Join-Path $RootPath $paths.WebAPI
    $dalPath = Join-Path $RootPath $paths.DAL
    $dalProjectPath = "..\$($paths.DAL.Split('\')[-1])"
    
    Write-Host "[$serviceName] Adding migration..." -ForegroundColor Yellow
    
    if (-not (Test-Path $webApiPath)) {
        Write-Host "  [SKIP] WebAPI path not found: $webApiPath" -ForegroundColor DarkGray
        continue
    }
    
    try {
        Push-Location $webApiPath
        
        $output = dotnet ef migrations add $MigrationName --project $dalProjectPath 2>&1
        $exitCode = $LASTEXITCODE
        
        if ($exitCode -eq 0) {
            Write-Host "  [OK] Migration added successfully" -ForegroundColor Green
            $SuccessCount++
            $Results += @{ Service = $serviceName; Status = "Success"; Message = "" }
        } else {
            $errorMsg = $output -join "`n"
            if ($errorMsg -match "No changes") {
                Write-Host "  [SKIP] No changes detected" -ForegroundColor DarkGray
                $Results += @{ Service = $serviceName; Status = "NoChanges"; Message = "" }
            } else {
                Write-Host "  [FAIL] $errorMsg" -ForegroundColor Red
                $FailCount++
                $Results += @{ Service = $serviceName; Status = "Failed"; Message = $errorMsg }
            }
        }
    }
    catch {
        Write-Host "  [ERROR] $($_.Exception.Message)" -ForegroundColor Red
        $FailCount++
        $Results += @{ Service = $serviceName; Status = "Error"; Message = $_.Exception.Message }
    }
    finally {
        Pop-Location
    }
    
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Success: $SuccessCount" -ForegroundColor Green
Write-Host "  Failed:  $FailCount" -ForegroundColor $(if ($FailCount -gt 0) { "Red" } else { "Green" })
Write-Host ""

if ($FailCount -gt 0) {
    Write-Host "Failed services:" -ForegroundColor Red
    $Results | Where-Object { $_.Status -eq "Failed" -or $_.Status -eq "Error" } | ForEach-Object {
        Write-Host "  - $($_.Service): $($_.Message)" -ForegroundColor Red
    }
    exit 1
}

exit 0
