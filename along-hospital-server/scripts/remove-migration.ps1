# Remove Migrations Script for Along Hospital Server
# Usage: .\remove-migration.ps1                                  # Remove ALL services
#        .\remove-migration.ps1 -Services @("Product", "Auth")   # Remove specific services
#
# Path auto-generated from service name following pattern:
#   DAL: {ServiceName}Service\{ServiceName}Svc.DAL

param(
    [Parameter(Mandatory = $false)]
    [string[]]$Services = @()
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
# Format: ServiceName = "Custom\Path.DAL"
$PathOverrides = @{
    # "MedicalService" = "MedicalService\MedicalServiceSvc.DAL"
}

# Function to get DAL path (auto-generate or use override)
function Get-DALPath($serviceName) {
    if ($PathOverrides.ContainsKey($serviceName)) {
        return $PathOverrides[$serviceName]
    }
    # Auto-generate path from service name
    return "${serviceName}Service\${serviceName}Svc.DAL"
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Remove Migrations" -ForegroundColor Cyan
Write-Host "  Services: $(if ($Services.Count -eq 0) { 'ALL' } else { $TargetServices -join ', ' })" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$SuccessCount = 0
$FailCount = 0

foreach ($serviceName in $TargetServices) {
    $dalRelativePath = Get-DALPath $serviceName
    $dalPath = Join-Path $RootPath $dalRelativePath
    $migrationsPath = Join-Path $dalPath "Migrations"
    
    Write-Host "[$serviceName] Removing migrations folder..." -ForegroundColor Yellow
    
    if (-not (Test-Path $migrationsPath)) {
        Write-Host "  [SKIP] Migrations folder not found" -ForegroundColor DarkGray
        continue
    }
    
    try {
        Remove-Item -Path $migrationsPath -Recurse -Force
        Write-Host "  [OK] Migrations folder removed" -ForegroundColor Green
        $SuccessCount++
    }
    catch {
        Write-Host "  [ERROR] $($_.Exception.Message)" -ForegroundColor Red
        $FailCount++
    }
    
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Summary: Success=$SuccessCount, Failed=$FailCount" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($FailCount -gt 0) { exit 1 }
exit 0
