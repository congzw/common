# a common project for any app

## todolist

- utility
- di
- module
- logging
- scoped hubs
- multi tenants
- dynamic area sites

## develop dynamic modules support site

- NbSites.Web
- NbSites.Web.Areas.Demo(Demo)

when develop and debug, add module projects references to NbSites.Web(or else, web have to copy module assemblies to the main site by hand)
when publish, remove module projects references from NbSites.Web(so the main site can load module assemblies dynamic without runtime error)

# change list

- 20200724 init projects