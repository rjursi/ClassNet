#ifndef SERVERSOCKETCLASS_H
#define SERVERSOCKETCLASS_H

#include <stdio.h>
#include <iostream>
#include <WinSock2.h>
#include <ws2tcpip.h>

#include <atlImage.h>

#pragma	comment(lib, "ws2_32")

#define BUFSIZE 65000
#define PORT 9000

using namespace std;

class server_socket_class {
private:
	// ���� ���� ����
	WSADATA wsaData;
	SOCKET sock;
	SOCKADDR_IN sock_addr;

	BOOL broadcast_enable = TRUE;

	// �ϴ��� ������ ���� ����
	SOCKADDR_IN recver_addr;
	int recver_addr_size = sizeof(recver_addr);

	char var_request[2] = "1";
	char var_response[2];

	// ���� ������ ���ۿ� ����� ����
	char *buf;

	FILE *filepointer;
	int file_size;
	int send_size;

	// ���� ����
	char msgbuf[2];

public:
	// ������
	server_socket_class();
	// �Ҹ���
	~server_socket_class();

	// Ŭ���̾�Ʈ ����
	void connect_client();

	// ������ ����
	void make_thread();

	// ��������
	void sendfile();
};

#endif // !SERVERSOCKETCLASS_H