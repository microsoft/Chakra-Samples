#include "ChakraCoreHost.h"

int main() 
{
	ChakraCoreHost host = ChakraCoreHost();
	host.runScript(host.loadScript(L"app.js"));
	return 0;
}
