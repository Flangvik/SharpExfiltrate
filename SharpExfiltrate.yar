rule SharpExfiltrateGUID
{
    meta:
        description = "The TypeLibGUID present in a .NET binary maps directly to the ProjectGuid found in the '.csproj' file of a .NET project."
        author = "Melvin Langvik (@flangvik)"
    strings:
        $typelibguid = "3bb553cd-0a48-402d-9812-8daff60ac628" ascii nocase wide
    condition:
        uint16(0) == 0x5A4D and $typelibguid
}


rule SharpExfiltrateFILE
{
    meta:
        description = "The cacheFileName is the name of the a file dropped by SharpExfiltrate when compressing large files"
        author = "Melvin Langvik (@flangvik)"
    strings:
        $cacheFileName = "SharpExfiltrateLootCache" ascii nocase wide
    condition:
        $cacheFileName
}