# Dataverse-CustomApis

This project contains a set of generic Dataverse Custom APIs that can be installed and consumed on any Dataverse environment.

The goal of the project is to enhance the capability of PowerPlatform developers and makers by providing robust and easy to use API that can be consumed agnostically by any calling mechanism. (Ex. Power Automate, PCF, Model-Driven Form javascript, ...)

Please submit any ideas for further addition to the collection in the [discussions](https://github.com/drivardxrm/Dataverse-CustomApis/discussions).

## Disclaimer 
> Custom API functionality is still considered as a Preview feature. While unlikely, some breaking changes might occur and will be fixed ASAP.

Here is the link to the Dataverse Custom API's [official documentation](https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/custom-api)

## Installation
Managed and unmanaged solutions are provided in the [Release section](https://github.com/drivardxrm/Dataverse-CustomApis/releases/)

## Current API list (see [Wiki](https://github.com/drivardxrm/Dataverse-CustomApis/wiki) for API Definitions)
### GetEnvironmentVariable 
This API is used to retrieve an Environment Variable value for a given key.
> Environment Variables are a great feature, but the lack of unified method to retrieve the values makes it somehow difficult to use. This API tries to simplify this process.  

see [API Definition](https://github.com/drivardxrm/Dataverse-CustomApis/wiki/GetEnvironmentVariable)


### GetUserTimezone
This API is Bound to the SystemUser Table and will provide easy access to the TimeZone code and TimeZone Standard name of a given user.
This is especialy usefull in PowerAutomate Flows for date conversion from UTC.à

see [API Definition](https://github.com/drivardxrm/Dataverse-CustomApis/wiki/GetUserTimezone)

### GetLocalizedChoiceLabel 
This API can be used to retrieve a choice (optionset) label for a known OptionsetValue in the langage code passed in parameter (given that the language is installed on the environment)

see [API Definition](https://github.com/drivardxrm/Dataverse-CustomApis/wiki/GetLocalizedChoiceLabel)


### GetTableInfo 
This API can be used to  retrieve some information on a table like 'CollectionName', 'SchemaName', "ObjectTypeCode'. The goal is not to replace a Metadata request but to surface some information more easilly (ex. via a custom action connector in Power Automate).

see [API Definition](https://github.com/drivardxrm/Dataverse-CustomApis/wiki/GetTableInfo)

