#pragma once

namespace BookCatalog
{
	namespace Utils
	{
		// Split a directory string into parts using a single delimiter
		std::vector<std::string> Split(const std::string& str, char delimiter);

		// Retrieve the hwnd of the console
        HWND GetConsoleHwnd(void);
	}
}
