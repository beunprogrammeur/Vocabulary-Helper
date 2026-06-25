Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "   LAUNCHING TRANSLATION SERVER          " -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan

$envFile = ".env"
$llamaPort = $null

if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        if ($_ -match "^LLAMA_PORT\s*=\s*(.*)$") {
            $llamaPort = $Matches[1].Trim().Trim('"').Trim("'")
        }
    }
}


if ([string]::IsNullOrEmpty($llamaPort)) {
    Write-Host "[!] LLAMA_PORT not defined. fallback to port 8080." -ForegroundColor DarkYellow
    $llamaPort = "8080"
}

Write-Host "[1/3] Booting Docker container on port $llamaPort..." -ForegroundColor Yellow
docker compose up -d

Write-Host "[2/3] Waiting for Aya Expanse to load into VRAM..." -ForegroundColor Yellow
$apiUrl = "http://localhost:$llamaPort/health"
$serverReady = $false
$attempts = 0
$maxAttempts = 30

while (-not $serverReady -and $attempts -lt $maxAttempts) {
    try {
        $response = Invoke-RestMethod -Uri $apiUrl -Method Get -TimeoutSec 2 -ErrorAction Stop
        if ($response.status -eq "ok" -or $response.status -eq "loading") {
            $serverReady = $true
        }
    }
    catch {
        $attempts++
        Start-Sleep -Seconds 2
        Write-Host "." -NoNewline -ForegroundColor Gray
    }
}

if ($serverReady) {
    Write-Host ""
    Write-Host "[3/3] Translation service is now running" -ForegroundColor Green
    Write-Host "Endpoint: http://localhost:$llamaPort/v1/chat/completions" -ForegroundColor Green
    Write-Host "=========================================" -ForegroundColor Cyan
} else {
    Write-Host ""
    Write-Host "[ERROR] Server failed to start within the time limit. Check 'docker logs llama-cpp-server'" -ForegroundColor Red
}