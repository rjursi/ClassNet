#ifndef SERVERSOCKETCLASS_H
#define SERVERSOCKETCLASS_H

#include <iostream>
#include <WinSock2.h>
#include <ws2tcpip.h>

#include <atlImage.h>

#pragma	comment(lib, "ws2_32")

#define BUFSIZE 65000
#define PORT 7777

using namespace std;

class server_socket_class {
private:
	// ���� ���� ����
	WSADATA wsaData;
	SOCKET sock;
	SOCKADDR_IN sock_addr;

	BOOL broadcast_enable = TRUE;

	// ���� ������ ���ۿ� ����� ����
	char *buf;

	FILE *filepointer;
	int file_size;
	int send_size;

	// ���� ����
	SOCKADDR_IN recver_addr;
	int recver_addr_size = sizeof(recver_addr);
	char msgbuf[2];

public:
	// ������
	server_socket_class();
	// �Ҹ���
	~server_socket_class();

	// ��������
	void sendfile();
};

#endif // !SERVERSOCKETCLASS_H