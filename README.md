# Localization module for Unity

This repository contains scripts for localization management in Unity.

## Table of contents

* [Quick start](#quick-start)
* [Feature list](#feature-list)
* [Bugs and feature requests](#bugs-and-feature-requests)
* [Tools](#tools)
* [Contributing](#contributing)
* [Versioning](#versioning)
* [Creators](#creators)
* [Copyright and license](#copyright-and-license)

## Quick start

To get the localization module you can:

* (Optional) Create a `Modules` folder in your project `Assets` folder. This folder will be a good place to store all of your modules.
* Clone the [utility module dependency](https://github.com/Kyappy/utility-module) `git clone https://github.com/Kyappy/utility-module`
* Clone the [repository](https://github.com/Kyappy/localization-module) `git clone https://github.com/Kyappy/localization-module.git`
* Create a `Localization` folder, preferably in a `Data` parent folder
* Inside the `Localization` folder, create subfolders for each local, the name of a local folder should match the following rules:
	* `language designator`: the code that represents a language using [two-letter ISO 639-1 standard](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes), example: `en`
	* `language designator` and `region designator`: the languqage designator followed by an hyphen and the code that represents a region using [ISO 3166-1 alpha-2 standard](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements), example: `en-US`

## Feature list

* Separates game text from game logic
* Supports Translations files organized into folder / file architecture
* Simple dot notation translation key access
* Supports any amount of languages
* Fallback local configuration and user local infered from system
* Parametters in translations
* Pluralization support
* Build-in component to support translation in Unity UI text component including properties and events binding

## Bugs and feature requests

If you have any request or you found an issue, please report it to the [Github issue tracker](https://github.com/Kyappy/localization-module/issues)

## Tools

* [C#](https://docs.microsoft.com/dotnet/csharp/) - Programming language
* [Unity 3D](https://unity3d.com/) - Game development plateform

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`

## Versioning

We use [Semantic Versioning guidelines](http://semver.org/) to maintain the project and ensure optimal compatibility. 

## Creators

This module is proposed by: [Marc Gavanier](https://github.com/Kyappy/), and developed by:
* [Marc Gavanier](https://github.com/Kyappy/)

## Copyright and license

Code and documentation copyright 2015-2017 [Marc Gavanier](https://github.com/Kyappy/) released under the [MIT License](http://opensource.org/licenses/MIT).
