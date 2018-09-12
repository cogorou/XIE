/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_io.h"
#include "IO/TxSerialPort.h"

namespace xie
{
namespace IO
{

// =================================================================
TxSerialPort::TxSerialPort()
{
	BaudRate		= 9600;
	Parity			= ExParity::None;
	DataBits		= 0;
	StopBits		= ExStopBits::One;
	Handshake		= ExHandshake::None;
}

// =================================================================
TxSerialPort::TxSerialPort(int buadrate, ExParity parity, int databits, ExStopBits stopbits, ExHandshake handshake)
{
	BaudRate		= buadrate;
	Parity			= parity;
	DataBits		= databits;
	StopBits		= stopbits;
	Handshake		= handshake;
}

// =================================================================
bool TxSerialPort::operator == (const TxSerialPort& cmp) const
{
	if (BaudRate		!= cmp.BaudRate) return false;
	if (Parity			!= cmp.Parity) return false;
	if (DataBits		!= cmp.DataBits) return false;
	if (StopBits		!= cmp.StopBits) return false;
	if (Handshake		!= cmp.Handshake) return false;
	return true;
}

// =================================================================
bool TxSerialPort::operator != (const TxSerialPort& cmp) const
{
	return !(operator == (cmp));
}

}
}
