# Read IFC and resave it to RVT with Design Automation

[![Design-Automation](https://img.shields.io/badge/Design%20Automation-v3-green.svg)](http://developer.autodesk.com/)

![Windows](https://img.shields.io/badge/Plugins-Windows-lightgrey.svg)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.8-blue.svg)
[![Revit-2022](https://img.shields.io/badge/Revit-2022-lightgrey.svg)](http://autodesk.com/revit)

## Forge DA Setup

### Activity via [POST activities](https://forge.autodesk.com/en/docs/design-automation/v3/reference/http/activities-POST/)

```json
{
    "commandLine": [
        "$(engine.path)\\\\revitcoreconsole.exe  /al \"$(appbundles[Ifc2Rvt].path)\""
    ],
    "parameters": {
        "ifcFile": {
            "verb": "get",
            "description": "Input IFC File",
            "required": true,
            "localName": "input.ifc"
        },
        "outputRVT": {
            "verb": "put",
            "description": "Output IFC RVT File",
            "localName": "output.ifc.rvt"
        }
    },
    "id": "youralais.Ifc2RvtActivity+dev",
    "engine": "Autodesk.Revit+2022",
    "appbundles": [
        "youralais.Ifc2Rvt+dev"
    ],
    "settings": {},
    "description": "Activity of resaving IFC to RVT",
    "version": 1
}
```

### Workitem via [POST workitems](https://forge.autodesk.com/en/docs/design-automation/v3/reference/http/workitems-POST/)

```json
{
    "activityId": "youralais.Ifc2RvtActivity+dev",
    "arguments": {
      "ifcFile": {
        "verb": "get",
        "url": "https://developer.api.autodesk.com/oss/v2/signedresources/...region=US"
      },
      "outputRVT": {
        "verb": "put",
        "url": "https://developer.api.autodesk.com/oss/v2/signedresources/...?region=US"
      }
    }
}
```

## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.

## Written by

Eason Kang [@yiskang](https://twitter.com/yiskang), [Forge Partner Development](http://forge.autodesk.com)
