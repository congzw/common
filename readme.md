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
- NbSites.Web.Boots
- NbSites.Web.Areas.Foo(Foo)

when develop and debug, add module projects references to NbSites.Web.Boots(if not so, web have to copy module assemblies to bin area by hand)
when publish, remove module projects references from NbSites.Web.Boots(so that site can load module assemblies dynamicly without rumtime error)

# change list

- 20200724 init projects