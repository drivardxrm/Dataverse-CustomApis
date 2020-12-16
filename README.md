# Dataverse-CustomApis
--WORK IN PROGESS--

This project contains a set of generic Dataverse Custom APIs that can be installed and consumed on any Dataverse environment.

The goal of the project is to enhance the capability of PowerPlatform developers and makers by providing robust and easy to use API that can be consumed agnostically by any calling mechanism. (Ex. Power Automate, PCF, Model-Driven Form javascript, ...)

Please submit any ideas for further addition to the collection in the [discussions](https://github.com/drivardxrm/Dataverse-CustomApis/discussions).

## Disclaimer 
> Custom Api functionality is still considered as a Preview feature. While unlikely, some breaking changes might occur and will be fixed ASAP.

Here is the link to the Dataverse Custom Apis [official documentation](https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/custom-api)

## Installation
Managed and unmanaged solutions are provided in the [Release section](https://github.com/drivardxrm/Dataverse-CustomApis/releases/)

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

**Usage**

**WebApi:**

`https://{{baseurl}}/api/data/v9.1/driv_GetEnvironmentVariable`
```
{
    "Key" : "{{SchemaName}}"
}
```

**Power Automate:**

![unbound-action-trigger](https://github.com/drivardxrm/Dataverse-CustomApis/blob/main/images/unboundaction.png)

![GetEnvironmentVariable-Flow-Request](https://github.com/drivardxrm/Dataverse-CustomApis/blob/main/images/GetEnvironmentVariable.png)

![GetEnvironmentVariable-Flow-Response](https://github.com/drivardxrm/Dataverse-CustomApis/blob/main/images/GetEnvironmentVariable_result.png)


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

**Usage**

**WebApi:**

`https://{{baseurl}}/api/data/v9.1/systemusers(00000000-0000-0000-0000-000000000000)/Microsoft.Dynamics.CRM.driv_GetUserTimezone`

**Power Automate:**

![bound-action-trigger](https://github.com/drivardxrm/Dataverse-CustomApis/blob/main/images/bound_action.png)

![GetUserTimezone-Flow-Request](https://github.com/drivardxrm/Dataverse-CustomApis/blob/main/images/GetUserTimezone.png)

![GetUserTimezone-Flow-Response](https://github.com/drivardxrm/Dataverse-CustomApis/blob/main/images/GetUserTimezone_result.png)

### GetLocalizedChoiceLabel (driv_GetLocalizedChoiceLabel)
This Api can be used to retrieve a choice (optionset) label for a known OptionsetValue in the langage code passed in parameter (given that the language is installed on the environment)

**Input Parameters**
| **Parameter**         | **Type**   |**Description**                                                                                  | **Required**     |
|-------------------|------   |----------------------------------------------------------------------------------------|----------   |
| **EntityName**               |String   |Entity Logical name                                            |    X         |
| **Attribute**               |String   |Attribute schema name                                           |    X         |
| **ChoiceValue**               |Integer   |Choice (optionset) integer value                                            |    X         |
| **LangCode**               |Integer   |Language code (ex. English = 1033, French = 1036 ...)                                           |    X         |

**Output Properties**
|**Property**         | **Type** | **Description**                                                                                  | 
|-------------------|-----|-----------------------------------------------------------------------------------------|
| **Exists**            |  Boolean   |  Returns true if the value exists                                           |
| **Value**            |  String   |  Label value retrieved                                           |

`https://{{baseurl}}/api/data/v9.1/driv_GetLocalizedChoiceLabel`
```
{
    "EntityName" : "{{EntityLogicalName}}",
    "EntityName" : "{{EntityLogicalName}}"

}
```


### GetTableInfo (driv_GetTableInfo)
This Api can be used to  retrieve some information on a table like 'CollectionName', 'SchemaName', "ObjectTypeCode'. The goal is not to replace a Metadata request but to surface some information more easilly (ex. via a custom action connector in Power Automate).

**Input Parameters**
| **Parameter**         | **Type**   |**Description**                                                                                  | **Required**     |
|-------------------|------   |----------------------------------------------------------------------------------------|----------   |
| **LogicalName**               |String   |Entity Logical name                                            |    X         |


**Output Properties**
|**Property**         | **Type** | **Description**                                                                                  | 
|-------------------|-----|-----------------------------------------------------------------------------------------|
| **Exists**            |  Boolean   |  Returns true if the Table exists                                           |
| **DisplayName**            |  String   |  DisplayName of the Table                                         |
| **SchemaName**            |  String   |  SchemaName of the Table                                         |
| **CollectionName**            |  String   |  CollectionName of the Table                                         |
| **CollectionSchemaName**            |  String   |  CollectionSchemaName of the Table                                         |
| **PrimaryIdAttribute**            |  String   |  PrimaryIdAttribute of the Table                                         |
| **PrimaryNameAttribute**            |  String   |  PrimaryNameAttribute of the Table                                         |
| **ObjectTypeCode**            |  Integer   |  ObjectTypeCode of the Table                                         |

**Usage**

**WebApi:**

`https://{{baseurl}}/api/data/v9.1/driv_GetTableInfo`
```
{
    "LogicalName" : "{{TableLogicalName}}"
}
```

