Get-ChildItem -Recurse -Filter *.cs | Select-String -Pattern '\[Table\("(\w+)"\)\]' | Select Path | ForEach-Object {
	$path = $_.Path
	$file = Get-Item $path

	$folderPath = Split-Path $file
	$entityName = $file.BaseName
	$configurationName = "$($entityName)Configuration"
	$configurationFileName = "$($configurationName).cs"
	$fullPath = Join-Path -Path $folderPath -ChildPath $configurationFileName

	New-Item $fullPath
	
	$tableName = Select-String -Path $file -Pattern '\[Table\("(\w+)"\)\]' -AllMatches | % { $_.Matches.Groups[1].Value }

	Add-Content $fullPath "using Microsoft.EntityFrameworkCore;"
	Add-Content $fullPath "using Microsoft.EntityFrameworkCore.Metadata.Builders;"
	Add-Content $fullPath ""
	Add-Content $fullPath "namespace Fred.EntityFramework"
	Add-Content $fullPath "{"
	Add-Content $fullPath "    public class $configurationName : IEntityTypeConfiguration<$entityName>"
	Add-Content $fullPath "    {"
	Add-Content $fullPath "        public void Configure(EntityTypeBuilder<$entityName> builder)"
	Add-Content $fullPath "        {"
	Add-Content $fullPath "            builder.ToTable(`"$tableName`");"
	Add-Content $fullPath "        }"
	Add-Content $fullPath "    }"
	Add-Content $fullPath "}"
}
