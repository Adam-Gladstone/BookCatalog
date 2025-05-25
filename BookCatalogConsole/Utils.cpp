#include "pch.h"

namespace BookCatalog 
{
	namespace Utils 
	{
		//
		// Split a directory string into parts using a single delimiter
		//
		std::vector<std::string> Split(const std::string& str, char delimiter)
		{
			std::vector<std::string> tokens;
			size_t start = 0;
			size_t end = str.find(delimiter);

			while (end != std::string::npos)
			{
				tokens.emplace_back(str.substr(start, end - start));
				start = end + 1;
				end = str.find(delimiter, start);
			}

			tokens.emplace_back(str.substr(start));
			return tokens;
		}

		// Retrieve the hwnd of the console
		//
		// https://learn.microsoft.com/en-us/troubleshoot/windows-server/performance/obtain-console-window-handle
		//
		HWND GetConsoleHwnd(void)
		{
			constexpr auto MY_BUFSIZE = 1024;		// Buffer size for console window titles.;
			HWND hwndFound;							// This is what is returned to the caller.
			char pszNewWindowTitle[MY_BUFSIZE];		// Contains fabricated WindowTitle.
			char pszOldWindowTitle[MY_BUFSIZE];		// Contains original WindowTitle.

			// Fetch current window title.
			GetConsoleTitleA(pszOldWindowTitle, MY_BUFSIZE);

			// Format a "unique" NewWindowTitle.
			sprintf_s(pszNewWindowTitle, "%d/%d", GetTickCount(), GetCurrentProcessId());

			// Change current window title.
			SetConsoleTitleA(pszNewWindowTitle);

			// Ensure window title has been updated.
			Sleep(40);

			// Look for NewWindowTitle.
			hwndFound = FindWindowA(NULL, pszNewWindowTitle);

			// Restore original window title.
			SetConsoleTitleA(pszOldWindowTitle);

			return(hwndFound);
		}
	}
}
