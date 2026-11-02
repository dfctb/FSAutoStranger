<img width="1280" height="640" alt="Untitled-1" src="https://github.com/user-attachments/assets/ba2d5330-bb85-4eba-9be4-e9670a42d805" />

# FS AutoStranger

A small BepInEx plugin for the Steam version of **Fallout Shelter** that automatically taps
the **Mysterious Stranger** when he appears in your vault, so you collect his Caps
without having to spot him yourself.

Unofficial fan-made mod. Not affiliated with or endorsed by Bethesda.

## Features
- Automatically taps the Mysterious Stranger as soon as he appears
- Hotkey to turn it on and off in game
- Safety pause between catches
- Optional testing key that summons the Stranger immediately

## Requirements
- Fallout Shelter (Steam, Windows)
- [BepInEx 5.4.x](https://github.com/BepInEx/BepInEx/releases) (Mono, x64)

Tested on Fallout Shelter running on Unity 6000.0.58 with BepInEx 5.4.23.2.
Works alongside other BepInEx plugins (for example [FS AutoHeal](https://github.com/dfctb/Failsafe-Auto-Heal-for-Fallout-Shelter)).

## Installation
1. Install BepInEx 5 into the game folder (next to `FalloutShelter.exe`) and start the game once.
2. Download `FSAutoStranger.dll` from the [Releases](../../releases) page.
3. Put it into `BepInEx/plugins`.
4. Start the game. The log (`BepInEx/LogOutput.log`) should contain `FS AutoStranger loaded`.

Back up your save before using any mods.

## Configuration
After the first launch, edit `BepInEx/config/com.conf.fsautostranger.cfg` (with the game closed):

| Section | Option | Default | Meaning |
|---|---|---|---|
| Hotkeys | ToggleKey | F6 | Turns auto-catching on/off in game |
| Hotkeys | SummonStrangerKey | (none) | Testing key: summons the Stranger right now |
| General | EnabledOnStart | true | Is it on when the game starts |
| General | CheckInterval | 0.5 | Seconds between checks |
| General | MinSecondsBetweenCatches | 30 | Safety pause after each catch |

When a catch happens, the log (`BepInEx/LogOutput.log`) gets the line `AutoStranger: Mysterious Stranger caught`.

## Known limitations
- PC (Steam) version only; mobile versions are not supported.

## Building from source
1. Install BepInEx into your game folder.
2. Copy `GameDir.user.props.example` to `GameDir.user.props` and set your game folder.
3. Build `FSAutoStranger.csproj` in Release mode (Visual Studio or `dotnet build -c Release`).
4. Copy `bin/Release/net472/FSAutoStranger.dll` to `BepInEx/plugins`.

The game's own DLLs are not included in this repository.

## License
CC0 1.0 Universal

---

## По-русски

Небольшой плагин для BepInEx для версии **Fallout Shelter** из Steam: автоматически нажимает на
**Таинственного незнакомца**, когда он появляется в убежище, чтобы вы получали его крышки,
не высматривая его самому.

**Установка:** поставьте BepInEx 5.4.x (Mono, x64) в папку игры, запустите игру один раз,
скачайте `FSAutoStranger.dll` из раздела Releases и положите в `BepInEx/plugins`.

**Настройки** лежат в `BepInEx/config/com.conf.fsautostranger.cfg` (редактируйте при закрытой игре):
клавиша включения/выключения (по умолчанию F6), необязательная тестовая клавиша вызова незнакомца,
интервал проверки и пауза между поимками.

**Ограничения:** работает только в версии для ПК (Steam). Перед использованием модов сделайте копию сохранений.
