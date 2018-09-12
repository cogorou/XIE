
#include <stdio.h>
#include <stdlib.h>
#include <stddef.h>

#include "xie_core.h"		// (1)
#include "xie_high.h"		// (1)
#include "xie.h"			// (1)

// ============================================================
int main(int argc, const char* argv[])
{
	xie::Axi::Setup();		// (2)

	// 
	// implementation
	// 

	xie::Axi::TearDown();	// (3)

	return 0;
}
