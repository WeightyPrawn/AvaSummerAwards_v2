# COOP API Management Azure DevOps Extension 
The extension includes two release tasks that can be found under the Deploy section:

***[COOP API Management - Deploy API](#coop-api-management---deploy-api)***

***[COOP API Management - Release Latest Revision](#coop-api-management---release-latest-revision)***

## COOP API Management - Deploy API
This task is used to create or update a versioned API in APIM. Including any policies and/or named values needed. It's also possible to have the task create a new revision instead of overwriting the current one.

### Configurable fields:

Display name* - *The display name of the task in your Azure Devops Stage.*

Azure RM Subscription* - *The service connection to the subscription your APIM resource belongs to.*

Resource Group* - *The Resource Group your APIM resource belongs to.*

API Management Service* - *Your APIM Resource*

Artifacts Base Path - *Path to where your artifacts are located ([See description](#artifacts-base-path) of artifacts in the next section below)*

API Name* - *The Name of your API in APIM*

API Display Name* - *The Display Name of your API in APIM*

Base URL* - *The URL to your backend service*

Product(s)* - *Select one or more existing Products to add your API to.*

Prefix NamedValues With ApiName - *If checked any named values added in the next field will have their name prefixed with the value you set for API Name in teh field above.*

Named Values - *Add any named values desired.*

Revisions (section header)

Create Revision - *Check to create a revision instead of overrwiting existing.*

(If Create Revision is checked) Revision description mode* - *Select if the revision description should be commit id or Release name and description from the Azure Devops release.*

(If Create Revision) Number of historic revisions to keep* - *How many historic revisions should be kept ([See example](#number-of-historic-revisions-to-keep) in the next section below).*

Versioning (section header)

API Version* - *Specify the version name.*

Versioning scheme* - *Query String, Header or Path*

(if Query String) Version Query Parameter* - *Specify the query parameter to use.*

(if Header) Version Header* - *Specify the name of the header*

### Artifacts Base Path

Required artifacts structure

    -swagger.json
    Policy (folder)
	    -all-operations.xml
	    -{operationId_1}.xml
	    -{operationId_2}.xml
	    ...

swagger.json needs to be in OpenAPI 2.0 format
xml files in Policy folder should be in standard APIM Policy format:

    <policies>
	    <inbound>
		    <base />
	    </inbound>
	    <backend>
		    <base />
	    </backend>
	    <outbound>
		    <base />
	    </outbound>
	    <on-error>
		    <base />
	    </on-error>
    </policies>

The policy definition from the file named "all-operations.xml" will be applied for all operations on the API.

To specify policies for a specific operation, specify the policy in a file named {operationId}.xml, where {operationId} is the operationId specified for the operation in the swagger.json.

### Number of historic revisions to keep

Ex. with value set to 2, first release will create the API with the first revision, revision 1, which will automatically be set to current.
Second and third release will create new revisions, revision 2 and 3.
On the fourth release a new revision, revision 4, will be created and the oldest non current revision will be removed, in this case revision 2.
If revision 2 or 3 had been set to current prior to release 4, revision 1 will be deleted instead.

## COOP API Management - Release Latest Revision
Use this task to relese the latest revision (make the latest revision current).

Can **ONLY** be used if one of the preceding tasks in the stage is a "COOP API Management - Deploy API" task as it will use the values specified for Resource Group, API Name, etc in that task.

### Configurable fields:
Display name* - *The display name of the task in your Azure Devops Stage.*

Azure RM Subscription* - *The service connection to the subscription your APIM resource belongs to.*
