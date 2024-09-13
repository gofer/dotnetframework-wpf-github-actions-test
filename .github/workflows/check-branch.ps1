$branches = git branch --contains $Args[0] | foreach { $_.Trim(' *') }
foreach ($branch in $branches) {
    if ($branch -eq $Args[1]) {
        exit 0
    }
}
exit 1
