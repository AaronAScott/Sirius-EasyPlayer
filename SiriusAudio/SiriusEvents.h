#pragma once

// SiriusEvents.h
#ifndef SIRIUS_EVENTS_H
#define SIRIUS_EVENTS_H

enum SEP_Event {
	SEP_None = 0,
	SEP_PlaylistLoading = 1,
	SEP_PlaylistLoaded = 2,
	SEP_SongChanged = 3,
	SEP_PlayStateChanged = 4,
	SEP_UnreadableByMA = 5,
	SEP_Error = 6,
	SEP_Count = 7   // always last; not an event
};

enum SEP_PlayState {
	SEP_Undefined = 0,
	SEP_Stopped = 1,
	SEP_Paused = 2,
	SEP_Playing = 3,
	SEP_PlayingExternal = 4,
	SEP_ScanForward = 5,
	SEP_ScanReverse = 6,
	SEP_MediaEnded = 7,
	SEP_PlaylistEnded = 8,
	SEP_Ready = 9
};

#endif

