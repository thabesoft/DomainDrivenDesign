# 检查是否输入了版本号
if ($args.Count -eq 0) {
    Write-Host "❌ 错误: 请输入版本号 (例如: 1.0.0-alpha.1)" -ForegroundColor Red
    exit
}

$Version = "v$($args[0])"

Write-Host "🚀 正在准备发布版本: $Version ..." -ForegroundColor Cyan

# 1. Git 提交
git add .
git commit -m "release: $Version"

# 2. 打标签
git tag -a "$Version" -m "Release $Version"

# 3. 推送代码和标签
Write-Host "📤 正在推送到远程仓库..." -ForegroundColor Yellow
git push origin main
git push origin "$Version"

Write-Host "✅ 完成！请前往 GitHub Actions 查看构建进度。" -ForegroundColor Green