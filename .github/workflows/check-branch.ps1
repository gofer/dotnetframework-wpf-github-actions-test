$branches = git branch --contains '${{ github.ref_name }}' | foreach { $_.Trim(' *') }
foreach ($branch in $branches) {
    if ($branch -eq 'feature-add-github-actions') {
        exit 0
    }
}
exit 1
