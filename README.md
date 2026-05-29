# Password API Assessment Tool

## Overview

This is a C# (.NET 8) console application built for an API-based assessment.

It automates the full flow of:

* password generation
* authentication via Basic Auth
* ZIP creation of submission package
* Base64 encoding
* upload to API endpoint

A SAFE MODE is included so the project can be fully tested locally without making any API calls.

---

## Project Structure

```
PasswordAPIAssessment/
│
├── Program.cs
├── Services/
│   ├── AuthClient.cs
│   ├── UploadClient.cs
│   ├── PasswordGenerator.cs
│   ├── ZipBuilder.cs
│
├── PasswordAPIAssessment.csproj
│
├── Assets/
│   ├── CV Sandeep Hari (Junior Dev).pdf
│   └── Password API Assessment – How Things Got Done (HTG).pdf
│
├── Data/
│   └── dict.txt (generated at runtime)
│
├── scripts/
│   ├── PushFirst.bat
│   └── PushUpdate.bat
│
└── .gitignore
```

---

## How It Works

1. Generate password permutations
2. Save dictionary file (dict.txt)
3. Attempt authentication against API using Basic Auth
4. On success, receive upload URL
5. Build ZIP containing:

   * source code
   * CV
   * dictionary file
6. Convert ZIP to Base64
7. Upload to API endpoint

---

## SAFE MODE vs LIVE MODE

SAFE MODE (default):

* No API calls are made
* ZIP is created locally for inspection
* Used for debugging and validation

LIVE MODE:

* Performs authentication
* Uploads final ZIP to API

```csharp
bool isLive = false;
```

---

## Scripts

The project includes automation scripts for Git workflow:

### PushFirst.bat

* Initial commit and push to GitHub repository

### PushUpdate.bat

* Used for subsequent commits and updates

These scripts help streamline development and maintain consistent commit history.

---

## Output Files

* dict.txt → generated password list (ignored in Git)
* PasswordAPIAssessment_Submission.zip → final packaged output (SAFE MODE only)
* CV Sandeep Hari (Junior Dev).pdf → included in final ZIP package
* Password API Assessment – How Things Got Done (HTG).pdf → included in Assets folder
* API response → upload result (LIVE MODE)

---

## .gitignore

Important exclusions:

* bin/
* obj/
* .vs/
* Data/dict.txt
* *.zip

This ensures only source code and documentation are tracked.

---

## Notes

* Ensure SAFE MODE is enabled before running locally
* Always verify ZIP contents before switching to LIVE MODE
* Keep scripts and structure consistent for reproducibility