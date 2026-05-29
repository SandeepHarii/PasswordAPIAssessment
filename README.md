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

## Important Documentation

👉 HTG (Project Explanation): [Open Here](Assets/HTG.pdf)
👉 CV: [Open Here](Assets/CV.pdf)

---

## Architecture Flow

Generator → Auth Client → Zip Builder → Encoder → Upload Client

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
│   ├── CV.pdf
│   └── HTG.pdf
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

* No external API calls are made
* ZIP is created locally for inspection
* Used for debugging and validation

LIVE MODE:

* Performs authentication against API
* Uploads final ZIP to endpoint

```csharp
bool isLive = false;
```

---

## Scripts

Helper scripts used during development (optional, not required for execution):

* PushFirst.bat → initial commit and push setup
* PushUpdate.bat → subsequent updates

---

## Output Files

* dict.txt → generated password list (ignored in Git)
* PasswordAPIAssessment_Submission.zip → final packaged output (SAFE MODE only)
* CV.pdf → included in final ZIP package
* HTG.pdf → project explanation document

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
* Keep structure consistent for reproducibility
* Do not commit build artifacts or IDE-specific files

---

## Final Note

This project demonstrates end-to-end automation of authentication, file packaging, and API submission using C#.
It is designed to be readable, modular, and easy to validate for assessment review purposes.
