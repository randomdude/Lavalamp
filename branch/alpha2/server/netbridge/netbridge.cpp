// This is the main DLL file.

#include "stdafx.h"

#include "netbridge.h"

namespace netbridge
{
	public ref class transmitterDriver
	{

		public: 
			
			transmitterDriver(System::String ^strPortName)
			{
				if (strPortName->IsNullOrEmpty(strPortName))
					throw gcnew badPortException;

				// Convert String in to a char* so we can send it to the driver
				System::Text::ASCIIEncoding ^myEncoding = gcnew System::Text::ASCIIEncoding();
				int byteCount = myEncoding->GetByteCount(strPortName) ;

				myseshdata.portname = (char*)malloc(byteCount+1);
				memset(myseshdata.portname, 0x00, sizeof(myseshdata.portname)+1);
				for( int i = 0; i< byteCount; i++)
					myseshdata.portname[i] =  (char)strPortName->ToCharArray()[i];

				// Now, open the port.
				if (!initPort(&myseshdata))
					throw gcnew cantOpenPortException();
			}

			bool portOpen()
			{
				return ( isPortOpen(&myseshdata) );
			}

			~transmitterDriver()
			{
				this->!transmitterDriver();
			}

			!transmitterDriver()
			{
				if (portOpen())
				{
					if (!closePort(&myseshdata))
					{
						// Not a lot we can do here if this fails.
						//System.Windows.Forms.MessageBox.Show("Cannot close port");
						throw gcnew cantClosePortException();
					}
				}
			}
	};
}