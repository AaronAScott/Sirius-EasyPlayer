# Sirius EasyPlayer

> **Note:** Sirius EasyPlayer requires **Microsoft SQL Server LocalDB** to be installed and available on the system.  
> The program will not function without LocalDB.

---

## Overview

Sirius EasyPlayer is a deterministic, library‑centric music player designed for listeners who care about structure, repeatability, and long‑term collection integrity more than visual flash or online integration. It focuses on:

- **Stable, predictable behavior** — the same inputs always yield the same outputs.
- **Local libraries only** — no streaming, no accounts, no cloud dependencies.
- **Clear, minimal interface** — library, playlists, and playback controls without clutter.
- **Respect for your files** — no silent rewriting, renaming, or “helpful” metadata edits.

It is especially well‑suited for large, carefully curated libraries (including classical collections) where multiple versions, formats, and masterings of the same work coexist.

---

## Key features

### Library management

- **Local file library:**
  - Scans user‑specified folders for supported audio formats.
  - Stores library metadata in a LocalDB database for fast, deterministic queries.
  - Does not modify audio files or embedded tags.

- **Deterministic indexing:**
  - Assigns stable internal IDs to tracks, albums, and playlists.
  - Ensures that sorting and selection remain consistent across sessions.

- **Format and extension awareness:**
  - Recognizes multiple encodings of the same track (e.g., FLAC, ALAC, MP3).
  - Uses a configurable extension/format ranking to prefer higher‑quality versions.

### Playback and fallback behavior

- **Deterministic playback engine:**
  - Plays tracks in a well‑defined, reproducible order.
  - No hidden shuffle or “smart” reordering unless explicitly requested.

- **Fallback architecture:**
  - If a preferred version of a track is unavailable (moved, offline drive, etc.), the player can fall back to alternate encodings of the same work when present.
  - Fallback rules are explicit and predictable, not heuristic.

- **Gapless, queue‑based playback:**
  - Uses a queue model for playback, derived from playlists or ad‑hoc selections.
  - Maintains clear visibility into “now playing” and “up next.”

### Playlists

- **Playlist editor:**
  - Create, rename, and delete playlists.
  - Add tracks from the library via search, browse, or context menus.
  - Reorder tracks with deterministic, index‑based ordering.

- **“Choose best version” for playlists:**
  - For playlists containing multiple encodings or duplicates of the same logical track, a **“Choose best version”** command is available from the playlist list box.
  - Applies your format/extension ranking and other rules to:
    - Prefer lossless over lossy.
    - Prefer primary storage over removable/secondary locations (if configured).
  - Cleans up playlists so each logical track is represented by the best available version.

- **Stable references:**
  - Playlists reference tracks by internal IDs, not by fragile path strings alone.
  - If files move within known roots, the player can often re‑resolve them without manual repair.

### Browsing, search, and filtering

- **Structured views:**
  - Browse by artist, album, and (optionally) work/composer for classical collections.
  - Album‑centric view that respects disc numbers and track ordering.

- **Search:**
  - Text search across titles, artists, albums, and other key fields.
  - Deterministic result ordering (e.g., by relevance, then by album/track index).

- **Filters:**
  - Filter by format, rating (if used), or other metadata fields.
  - Filters are order‑preserving: they narrow the set without re‑scrambling it.

### Album art and metadata

- **Album art display:**
  - Shows embedded artwork when available.
  - Can fall back to folder‑based images (e.g., `folder.jpg`) when configured.

- **Metadata respect:**
  - Reads tags for display and organization.
  - Does not write or “fix” tags automatically.

### User interface and behavior

- **Minimal, focused UI:**
  - Main window with library view, playlist pane, and playback controls.
  - Context menus for common actions (add to playlist, play next, locate in library, choose best version, etc.).

- **Deterministic sorting:**
  - Sorting rules are explicit and stable (e.g., album sort by artist, year, disc, track).
  - No hidden “smart” grouping that changes between runs.

- **Help → About:**
  - Displays program version, basic environment information, and license information.
  - Includes a combined license view for Sirius EasyPlayer and third‑party components (generated from individual license fragments).

---

## Requirements

- **Operating system:**
  - Windows 10 or later (desktop).
- **Database:**
  - **Microsoft SQL Server LocalDB** (required).
- **Runtime:**
  - .NET Desktop runtime (version TBD by final build).
- **Storage:**
  - Sufficient disk space for:
    - The LocalDB database.
    - Your audio files (stored wherever you choose).

---

## Installation

1. **Install LocalDB:**
   - Ensure Microsoft SQL Server LocalDB is installed and accessible on your system.
   - Verify that the LocalDB instance can be started under your user account.

2. **Install required runtime:**
   - Install the appropriate .NET Desktop runtime if not already present.

3. **Obtain Sirius EasyPlayer:**
   - Download the release package (ZIP or installer) from the project’s distribution location.

4. **Run the program:**
   - Launch `SiriusEasyPlayer.exe`.
   - On first run, the program will:
     - Initialize or upgrade the LocalDB database.
     - Prompt you to select one or more library root folders.

---

## First‑run setup

1. **Select library folders:**
   - Choose the directories where your music is stored.
   - You can add multiple roots (e.g., internal drive plus external archive).

2. **Initial scan:**
   - The program scans the selected folders and populates the LocalDB library.
   - Progress is shown in the status area; you can begin exploring once the initial pass is complete.

3. **Configure format preferences:**
   - Set your preferred format/extension ranking (e.g., `FLAC > ALAC > WAV > MP3`).
   - These preferences drive the “choose best version” logic and fallback behavior.

4. **Optional: UI preferences:**
   - Adjust view options (e.g., show/hide columns, default sort order).
   - Save layout so it remains consistent across sessions.

---

## Usage highlights

- **Playing an album:**
  - Navigate to the album view.
  - Double‑click an album or use the context menu to “Play album.”
  - Tracks play in album order, respecting disc and track indices.

- **Creating a playlist:**
  - Open the playlist pane and create a new playlist.
  - Drag tracks from the library or use “Add to playlist” from context menus.
  - Use the playlist list box to reorder tracks as needed.

- **Cleaning up a playlist with multiple versions:**
  - Open the playlist.
  - Use the **“Choose best version”** command from the playlist list box menu.
  - The playlist is updated so each logical track uses the best available encoding according to your preferences.

- **Handling missing files:**
  - If a track is unavailable, the player:
    - Attempts to locate alternate encodings of the same track.
    - Applies fallback rules deterministically.
  - Missing items are clearly indicated; nothing is silently skipped without explanation.

---

## Design principles

- **Determinism over cleverness:**
  - Every operation has a defined, repeatable outcome.
- **Local control:**
  - No network dependencies for core functionality.
- **Transparency:**
  - Clear indication of what the player is doing and why (especially for fallback and version selection).
- **Non‑destructive:**
  - Your files and tags are treated as read‑only data.

---

## Known limitations

- **LocalDB dependency:**
  - The program will not run without a working LocalDB installation.
- **Local libraries only:**
  - No streaming services or online metadata lookups.
- **Single‑user focus:**
  - Designed for a single user on a single machine; no multi‑user or sync features.

---

## License

Sirius EasyPlayer is distributed under the **[LICENSE NAME HERE]** license.  
The **Help → About → Licenses** section in the program presents:

- The main Sirius EasyPlayer license.
- A combined, generated view of all third‑party component licenses.

(Replace this section with the actual license text and details before release.)

---

## Contributing, issues, and feedback

At this stage, Sirius EasyPlayer is focused on stability and deterministic behavior. If you encounter:

- **Bugs or crashes**
- **Unexpected ordering or fallback behavior**
- **Incorrect “best version” selections**

please document:

- What you were doing.
- The relevant library/playlist context.
- Any error messages shown.

and report them through the project’s chosen issue tracker or feedback channel.

---