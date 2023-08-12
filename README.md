CCEM is an "easy mode" mod for the (very NSFW) indie unity game [_Cowtastic Cafe_](https://noa3.itch.io/cowtastic). Installation is extremely simple - [download the latest build](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/releases/latest), unzip it into the game's folder, and just launch the game.

All features are controlled via hotkeys, with both the on/off status and keybind displayed in the lower right corner. Settings are saved in `EasyMode.properties` to persist between game launches. Please note that this display was designed for a 1920x1080 game window (fullscreen on such a monitor) and may look off on other resolutions. Current features include:

- prevent happiness from falling below 5% (so you can't lose, but still need to try and keep it up manually)
- lock happiness at 100% (if you just don't want to deal with it)
- multiply income by 2x (compounds with 5x, both will give 10x)
- multiply income by 5x (compounds with 2x, both will give 10x)
- reduce all spent money by half (you still need to HAVE the full amount, but only half the cost is spent)
- disable spending entirely (you still need to HAVE the full amount, but no money is spent)
- guarantee perfect order ratings (whatever you give the customer will be rated as perfect)
- autofill cup when customer orders (cup will start with the exact order, ONLY applies when an order is STARTED)
- status overlay in the lower right corner to display what features are enabled and what hotkeys you can use to toggle them

Some features support multiple keybinds, but only one (the key that's unique to that feature) is displayed in the overlay. There are a few extra keybinds that toggle multiple settings at once. **NOTE**: these keys _toggle_ each of the settings they connect to, so if one is off and the other is on, pressing it will end with them flipped around, not both set to on/off!
- `H`: happiness minimum threshold, lock happiness at cap
- `I`: 2x income, 5x income
- `O`: half spending, no spending
- `M`: 2x income, 5x income, half spending, no spending

Tips:
- the "lock happiness at cap" feature actually sets happiness to 100% and just repeats that every frame, so you can turn it on and then back off to just get an instant boost to full and then allow it to decay as normal
- autofill cup only works when the order is started, but perfect ratings has no such limit, so if you suddenly get an order too complicated to fill in time, you can just turn perfect ratings on and serve anything - even nothing but 1% coffee!
- the status overlay is unrelated to the rest of the features and their hotkeys; if you want to leave it hidden, you can, and everything else will continue to work - INCLUDING hotkeys toggling other features

It is _technically_ safe to update the game without updating CCEM, since CCEM doesn't replace any of the game's own files. However, game updates _may_ break CCEM - in the worst case, the game won't even launch, but that is very unlikely. If you experience any problems, you can **remove `winhttp.dll` from the game folder** in order to disable CCEM.

If you encounter any issues, please [open a report on the tracker](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/issues/new) to let me know and I'll see what I can do.
