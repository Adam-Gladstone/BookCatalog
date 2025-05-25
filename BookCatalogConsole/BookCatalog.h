#pragma once


namespace BookCatalog
{
	struct Book
	{
		explicit Book(const std::filesystem::path& p) 
		{
			m_filename = p.string();
			m_title = p.stem().string();
		
			std::vector<std::string> parts = Utils::Split(m_filename, '\\');
			if ( (parts.size() > 0) && ( (parts.size() - 2) > 0) )
			{
				m_category = parts.at(parts.size() - 2);
			}
			else
			{
				std::cout << "Unable to find book category on the path.\n";
			}
		}

		const std::string& Filename() const { return m_filename; }

		const std::string& Title() const { return m_title; }

	private:
		std::string m_filename;
		std::string m_title;
		std::string m_category;
	};

	// Find all book titles containing the search term
	std::size_t FindAll(std::string term);

	// Search
	bool OnSearch(const std::string& prompt);

	// Open
	bool OnOpen(const std::string& prompt);

	// Close the console application
	bool OnClose(const std::string& prompt);
}
