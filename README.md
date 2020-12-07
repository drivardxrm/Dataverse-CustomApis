# Dataverse-CustomApis
--WORK IN PROGESS--

This project contains a set of generic Dataverse Custom APIs that can be installed and consumed on any Dataverse environment.

The goal of the project is to enhance the capability of PowerPlatform developers and makers by providing robust and easy to use API that can be consumed agnostically by any calling mechanism. (Ex. Power Automate, PCF, Model-Driven Form javascript, ...)
  

## Disclaimer 
> Custom Api functionality is still considered as a Preview feature. While unlikely, some breaking changes might occur.

Here is the link to the official documentation : https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/custom-actions

## Current API list
### GetEnvironmentVariable (driv_GetEnvironmentVariable)
This Api is used to retrieve an Environment Variable value for a given key.
> Environment Variables are a great feature, but the lack of unified method to retrieve the values makes it somehow difficult to use. This API tries to simplify this process.  

**Input Parameters**
| **Parameter**         | **Type**   |**Description**                                                                                  | **Required**     |
|-------------------|------   |----------------------------------------------------------------------------------------|----------   |
| **Key**               |String   |SchemaName of the Environment Variable definition                                            |    X         |

**Output Properties**
|**Property**         | **Type** | **Description**                                                                                  | 
|-------------------|-----|-----------------------------------------------------------------------------------------|
| **Exists**            |  Boolean   |  Returns true if the value exists                                           |
| **ValueString**            |  String   |  The string value of the environment variable. (Always populated)                                           |
| **ValueDecimal**           |  Decimal   |  The decimal value of the environment variable if the Type is 'Decimal'                                         |
| **ValueBool**            |  Boolean   |  The boolean value of the environment variable if the Type is 'Boolean'                                            |
| **Type**            |  Picklist   |  Returns the optionset value of the Environment Variable type. (String-100000000, Number-100000001, Boolean-100000002,JSON-100000003) |
| **TypeName**            |  String   |  Return the label of the Environment Variable type   |                                       |




---
### GetUserTimezone (driv_GetUserTimezone)
This API is Bound to the SystemUser Table and will provide easy access to the TimeZone code and TimeZone Standard name of a given user.
This is especialy usefull in PowerAutomate Flows for date conversion from UTC.à

**Bound Table**
SystemUser

**Input Parameters**
*--none--*

**Output Properties**
|**Property**         | **Type** | **Description**                                                                                  | 
|-------------------|-----|-----------------------------------------------------------------------------------------|
| **TimezoneCode**            |  Integer   |  Timezone code                                           |
| **TimezoneName**            |  String   |  Timezone Standard Name (Use this value in Power Automate date conversion                                           |


## Installation
Managed and unmanaged solutions are provided in the Release section : https://github.com/drivardxrm/Dataverse-CustomApis/releases/

