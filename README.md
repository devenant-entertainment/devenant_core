# Devenant Core
Base project to all devenant games and applications.
##Setup
1. Download Devenant Core repository to your local machine on the master branch.
2. Create a new Unity  project and import the Devenant Core package from disk using package manager.
3. Download and import the Steamworks.NET package form https://steamworks.github.io/installation/.
## Modules
//TODO
## Localization Base Tables
It is recommended to use a translation table for each element or data in the game. For example: races, classes, characters, statistics, states, etc. For short user interface elements, it is recommended to use the taken "interface" table. For data tables it is advisable to enter "_name" or "_desc" at the end of the key as shown in the following examples:
### Achievement (Example)
| Table | Key | Sample |
| --- | --- | --- |
| achievement | kill_boss_name | Are you sure you want to logout? |
| achievement | kill_boss_desc | Your account has been banned. Do you want to contact support? |
### Interface (Example)
This table is for base messages sent by Devenant Server and Devenant Core.
| Table | Key | Sample |
| --- | --- | --- |
| interface | accept | Accept |
| interface | cancel | Cancel |
| interface | no | No |
| interface | yes | Yes |
### Messages (Required)
This table is for base messages sent by Devenant Server and Devenant Core. It is recommended to use prefix like "dialogue" or "error" to categorize the messages.
| Table | Key | Sample |
| --- | --- | --- |
| message | dialogue_exit | Are you sure you want to exit game? |
| message | dialogue_logout | Are you sure you want to logout? |
| message | dialogue_user_banned | Your account has been banned. Do you want to contact support? |
| message | dialogue_user_deleted | This account is deleted. Do you want to recover it? |
| message | dialogue_user_unvalidated | Your account is unvalidated. Do you want to activate it? |
| message | dialogue_version | There is a new game version. Do you want to update game? |
| message | error | An unknown error has ocurred. |
| message | error_field_empty | You must fill all fields. |
| message | error_field_email | You must enter a valid email. |
| message | error_field_nickname | You must enter a valid nickname with a minimum of 2 characters. |
| message | error_field_legal | You must accept the terms and conditions of the service. |
| message | error_field_password | You must enter a valid password. It must be a combination of uppercase and lowercase letters and numbers, with a minimum of 8 characters and a maximum of 32. |
| message | error_maintenance | Game servers are in maintenance. Try again later. |
| message | error_user_code | Invalid user code. |
| message | error_user_login | There was an error during login. |
| message | error_user_register | There was an error during register. |
| message | error_user_registered_email | The email is taken. |
| message | error_user_registered_nickname | The nickname is taken. |
| message | error_user_send_code | There was an error during code sending. |
| message | error_user_token | Invalid user token. |
| message | error_user_update | There was an error during user data update. |
| message | info_user_code | User code was sent to your email. |
| message | info_user_deleted | User account was deleted. |
| message | info_user_registered | User account was registered. |
| message | info_user_update_email | User email was updated. |
| message | info_user_update_nickname | User nickname was updated. |
| message | info_user_update_password | User password was updated. |