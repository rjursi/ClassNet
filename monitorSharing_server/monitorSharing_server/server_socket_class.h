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
	// 소켓 관련 변수
	WSADATA wsaData;
	SOCKET sock;
	SOCKADDR_IN sock_addr;

	BOOL broadcast_enable = TRUE;

	// 일대일 연결을 위한 변수
	SOCKADDR_IN recver_addr;
	int recver_addr_size = sizeof(recver_addr);

	char var_request[2] = "1";
	char var_response[2];

	// 파일 데이터 전송에 사용할 변수
	char *buf;

	FILE *filepointer;
	int file_size;
	int send_size;

	// 응답 변수
	char msgbuf[2];

public:
	// 생성자
	server_socket_class();
	// 소멸자
	~server_socket_class();

	// 클라이언트 연결
	void connect_client();

	// 스레드 생성
	void make_thread();

	// 파일전송
	void sendfile();
};

#endif // !SERVERSOCKETCLASS_H