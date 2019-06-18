# Sitecore Reports Module

This is the massively upgraded Sitecore Advanced System Reporter module, originally developed by [Raul Jimenez](https://github.com/rauljmz) a very long time ago. This module was born circa 2008, way back when Sitecore was still on version 6. Holy crap, that's like a decade ago.

It is currently compatible with Sitecore 9.1, sort of. There are some aesthetic and usability issues but generally it works.

## Further Improvements
Where do I even start? There's a lot that could be done.
* Fix aesthetic and usability issues, for example opening a report is clunky
* Retest all building blocks - scanners, filters and viewers
* Re-engineer the user interface with [Speak](https://doc.sitecore.com/developers/speak/90/speak/en/introducing-speak.html). It's a big job!
* Implement a better report designer. This was always on the wish list

### Who is this module for?
The module is **intended for business or non-technical users of Sitecore**. It aims to provide user friendly way of running reports and exporting data, specifically focused to assist marketing and content authoring teams.

### Are there any alternatives?
Yes, there's certainly a few.

#### Sitecore PowerShell Extensions
[Sitecore PowerShell Extensions](https://doc.sitecorepowershell.com) is another module that provides similar capability (reporting) plus a lot of other more advanced features. If you primarily need to develop reports for technical users of Sitecore, I would recommend you use Sitecore PowerShell Extensions.

#### Sitecore Log Analyzer
[Sitecore Log Analyzer](https://marketplace.sitecore.net/Modules/Sitecore_Log_Analyzer.aspx?sc_lang=en) is a specific tool designed to analyse Sitecore log files.


### So, why did I decide to resurrect this module from the dead?
Well it was never really dead, just lurking in the shadows. Over the years I had more than one client ask for a reporting solution and I gradually upgraded the module for each Sitecore version. I never got around to pushing the upgraded and improved version back to GitHub - lack of time is always a good excuse.

A few minor improvements were added to the module here and there, mostly to add more flexibility for scanning, filtering and viewing. The last major upgrade also focused on aligning the module with the Helix Framework principles to allow it to be used alongside other Helix based modules.

## Notes

#### Unicorn
Use of TDS (Team Development for Sitecore) for item synchronisation has been replaced with Unicorn as it is available for free and does not require a paid license, which many developers didn't have access to.

#### Patches for older versions
The following Sitecore support patch for Sitecore CMS 8.2 rev. 161221 (Update-2) has been incorporated into the module.
The issue was directly affecting the ability to export and download reports.

https://github.com/SitecoreSupport/Sitecore.Support.90534/releases/

