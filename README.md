# GitGud
A simple unity-based git client with a focus on extendability 

<img src="https://i.imgur.com/4RTQf6j.png">

### Features 
* Simple settings
* Stage / Unstage
* Icons
* Attribute-based hooks
* Basic commit history
* Committing

### Features Coming Soon
* Redesign Error catching
* Pushing / Pulling
* Better interface

### Wanted Features Someday
* Branching
* LFS locking
* Merge conflict resolvers
* Plugin API
* File tree visual
* File tree filters

## Attribute Hooks
There are currently 4 ways to hook into GitGud:

**PathContext:**

Adds a context option to specific fileviewers. (Currently staged and unstaged)
<img src="https://i.imgur.com/KmGtJix.png">
 
**CommitContext:**

Adds a context option to a specific commit list (Currently history only)
<img src="https://i.imgur.com/DYwZUsD.png">

**Tab:**

Adds a tab to the GitGud window.
<img src="https://i.imgur.com/3gQt4Ip.png">
         
**TopButton:**

Adds a button to the top bar in the GitGud window.
<img src="https://i.imgur.com/wKpw5Rh.png">

***Documentation to come soon***

# Design Goals
The goal of this tool is to keep things light - a base interface, with all of the commands needed to make it work. However, just keeping things light would be boring! Thus, I'm designing the tool to be as extendable as possible, so new features can be added. I plan on creating several plugins myself, such as a meta file manager (hiding away those pesky meta files, an resolving conflicts related to them) - of course, these plugins will be kept separate from the main repo

Since this project just started, I haven't written any docs yet. When the api starts to find solid ground, I'll be sure to do that - documentation is very important ^_^

