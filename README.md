[![Support me!](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/V7V7IK9UU)

## About

[![Game version: 1.1.0.1](https://img.shields.io/badge/game%20version-1.1.0.1-blue)](https://noa3.itch.io/cowtastic)
[![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/PrincessRTFM/CowtasticCafeEasyMode?label=mod%20version&color=informational)](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/releases/latest)
[![Latest release (date)](https://img.shields.io/github/release-date/PrincessRTFM/CowtasticCafeEasyMode)](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/releases/latest)
[![Total downloads](https://img.shields.io/github/downloads-pre/PrincessRTFM/CowtasticCafeEasyMode/total?label=downloads)](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/releases)
[![GitHub issues](https://img.shields.io/github/issues-raw/PrincessRTFM/CowtasticCafeEasyMode?label=known%20issues)](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)

CCEM is an "easy mode" mod for the (very NSFW) indie unity game [_Cowtastic Cafe_](https://noa3.itch.io/cowtastic). Installation is extremely simple - [download the latest build](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/releases/latest/download/Release.zip), unzip it into the game's folder, and just launch the game.

## Usage

All features are controlled via hotkeys, with the keybind displayed in the lower right corner. Please note that this display was designed for a 1920x1080 game window (fullscreen on such a monitor) and may look off on other resolutions. Some features support multiple keybinds, but only one (the key that's unique to that feature, first one listed) is displayed in the overlay. Most features are automatic and will be toggled by their hotkey; these will display their on/off status in the overlay. Settings are saved in `EasyMode.properties` to persist between game launches.

### Toggle settings

- \[`P`, `H`\] prevent happiness from falling below 5% (so you can't lose, but still need to try and keep it up manually)
- \[`L`, `H`\] lock happiness at 100% (if you just don't want to deal with it)
- \[`E`, `I`, `M`\] multiply income by 2x (compounds with 5x, both will give 10x)
- \[`D`, `I`, `M`\] multiply income by 5x (compounds with 2x, both will give 10x)
- \[`R`, `O`, `M`\] reduce all spent money by half (you still need to _have_ the full amount, but only half the cost is spent)
- \[`N`, `O`, `M`\] disable spending entirely (you still need to _have_ the full amount, but no money is spent)
- \[`G`\] guarantee perfect order ratings (whatever you give the customer will be rated as perfect)
- \[`F`\] autofill cup when customer orders (cup will start with the exact order, any existing order will be auto-filled)
- \[`/`, `?`\] status overlay in the lower right corner to display what features are enabled and what hotkeys you can use to toggle them

#### IMPORTANT NOTE

The above keybinds toggle _all_ of the settings they connect to! This means that if you use `H` while happiness-lock-at-cap is enabled and happiness-minimum-threshold is disabled, you will end up with them flipped around, _not_ with both enabled/disabled.

### Immediate actions

- \[`[space]`\] immediately fill the current cup with the desired order
- \[`[PageUp]`, `[Home]`, `[Insert]`\] add $10, $100, or $1000 (respectively)
- \[`[PageDown]`, `[End]`, `[Delete]`\] remove $10, $100, or $1000 (respectively) - money will never go negative

## Tips

- the "lock happiness at cap" feature actually sets happiness to 100% and just repeats that every frame, so you can turn it on and then back off to just get an instant boost to full and then allow it to decay as normal
- the status overlay is unrelated to the rest of the features and their hotkeys; if you want to leave it hidden, you can, and everything else will continue to work - INCLUDING hotkeys toggling other features
- since all features can be disabled, you don't actually need to completely disable CCEM if you want to play a vanilla run

## Updating

It is _technically_ safe to update the game without updating CCEM, since CCEM doesn't replace any of the game's own files. However, game updates _may_ break CCEM - in the worst case, the game won't even launch, but that is very unlikely. If you experience any problems, you can **remove `winhttp.dll` from the game folder** in order to disable CCEM.

## Bugs

There are no bugs currently known. If you encounter any issues, please [open a report on the tracker](https://github.com/PrincessRTFM/CowtasticCafeEasyMode/issues/new/choose) (including your `EasyMode.log` file!) and I'll see what I can do. If you have a feature request, you can open an issue to tell me about it.

## Plans

I'd like the make the overlay in the bottom right corner look a little less awful, because it's pretty bad right now. Plain white text, plus it overlaps buttons in the main menu... not to mention it definitely won't look right on other resolutions. Ideally, I'd like to make it detect the button(s) in that corner and adjust to place itself right above them.
