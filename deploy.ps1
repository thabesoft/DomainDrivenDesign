# Force UTF8 encoding for console output
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Check for version argument
if ($args.Count -eq 0) {
    Write-Host "Error: Version number is required (e.g., 1.0.0-alpha.1)" -ForegroundColor Red
    exit
}

$VerNum = $args[0]
$TagName = "v$VerNum"

Write-Host "--- Starting Release Process: $TagName ---" -ForegroundColor Cyan

# 1. Build Check
Write-Host "Step 1: Checking build status..." -ForegroundColor Gray
dotnet build -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Please fix errors before releasing." -ForegroundColor Red
    exit
}

# 2. Cleanup Old Tags
Write-Host "Step 2: Cleaning up existing local/remote tags..." -ForegroundColor Gray
git tag -d $TagName 2>$null
git push origin --delete $TagName 2>$null

# 3. Commit Changes
Write-Host "Step 3: Committing changes to Git..." -ForegroundColor Gray
git add .
# Suppress error if there's nothing to commit
git commit -m "release: $TagName" 2>$null

# 4. Create New Tag
Write-Host "Step 4: Creating new git tag: $TagName" -ForegroundColor Yellow
git tag -a $TagName -m "Release $TagName"

# 5. Push to Remote
Write-Host "Step 5: Pushing code and tags to origin..." -ForegroundColor Yellow
git push origin main
git push origin $TagName

Write-Host "`n[SUCCESS] Release task submitted! Check GitHub Actions for progress." -ForegroundColor Green