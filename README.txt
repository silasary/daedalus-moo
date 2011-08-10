The Daedalus MOO client is based off the Chiroptera MUD client (Created by Tomi Valkeinen)

Note to developers:
The Original Chiroptera code has been kept in the repository for reference reasons.  It will be removed when the base client is complete.

The folders you should be working with are:
* Daedalus - This is the client itself. It contains all relevant frontend code.  This is the place most of your edits should be going.

Folders you might need to work with:
* ChiropteraBase - This is the core library, handling the telnet connection, ansi, and other core elements.  This is a required dependancy of Daedalus.
* Updater - It's my generic updater.  There is no real need to play around with it.

Folders you shouldn't be working with:
* ChiropteraWin - The old client.  There a handful of modifications made before I realized I should just rewrite the entire frontend, but for the most part it's the original client.  You should edit Daedalus instead.
* ChiropteraLin - A half-hearted linux frontend. I've not tested whether it works.  Don't bother with it.
* libchiroptera - Apparently related to the above linux frontend.

Other Folders:
* IronPython - The IronPython dlls.
* Scripts - The Chiroptera python scripts.  Daedalus currently doesn't support them, but should do so again in the future.