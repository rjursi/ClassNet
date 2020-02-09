#include "server_socket_class.h"
#include <time.h>

server_socket_class::server_socket_class() {

	packet_timeToLive = TTL;


	WSAStartup(MAKEWORD(2, 2), &wsaData);

	/// 버전이 지원하는 Word 크기로, 0000001000000010 =  514
	/// wsaData : Window 소켓 구현의 세부사항을 수신하는 WSADATA 데이터 구조 포인터



	sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP);
	// PF_INET - IPv4 의미, 
	// SOCK_DGRAM - 데이터 그램 전송 지원
	// IPPROTO_UDP - UDP 전송 지원

	memset((char*)&groupSock, 0, sizeof(groupSock));

	groupSock.sin_family = AF_INET;
	groupSock.sin_addr.s_addr = inet_addr("239.1.1.2");
	groupSock.sin_port = htons(PORT);

	
	localInterface.s_addr = inet_addr("192.168.0.187");
	//localInterface.s_addr = inet_addr("172.18.68.161");
	//localInterface.s_addr = htonl(INADDR_ANY);
	if (setsockopt(sock, IPPROTO_IP, IP_MULTICAST_IF, (char*)&localInterface, sizeof(localInterface)) < 0) {
		perror("Setting local interface Error");
		exit(1);	
	}
	else {
		printf("Setting the local interface... OK\n");
	} 



}

server_socket_class::~server_socket_class() {
	closesocket(sock);
	WSACleanup();
}

void server_socket_class::connect_client() {
	//sendto(sock, (char*)&var_request, sizeof(var_request), 0, (const SOCKADDR*)&sock_addr, sizeof(sock_addr));


	sendto(sock, (char*)&var_request, sizeof(var_request), 0, (const SOCKADDR*)&groupSock, sizeof(groupSock));
	// 버퍼에 있는 데이터 만큼 소켓, 즉 목적지로 보냄 (Broadcast)


	ZeroMemory(var_response, sizeof(var_response));
	// 응답 버퍼를 비움


	recvfrom(sock, var_response, sizeof(var_response), 0, (SOCKADDR*)&recver_addr, &recver_addr_size);
	// 소켓으로부터 들어오는 데이터를 해당 var_response 버퍼에 저장, 해당 받은 클라이언트 정보를 recver_addr 에 저장

}	

void server_socket_class::sendfile() {
	clock_t start, end;

	// 파일 열기
	filepointer = fopen("ScreenCapture.jpeg", "rb");
	fseek(filepointer, 0, SEEK_END); // 파일 마지막 위치로 이동


	file_size = ftell(filepointer);
	// 파일 포인터의 현재 위치를 반환


	rewind(filepointer);
	// 파일 포인터를 제일 앞으로 이동


	sendto(sock, (char*)&file_size, sizeof(file_size), 0, (const SOCKADDR*)&recver_addr, sizeof(recver_addr));
	/// 파일 크기에 대한 정보를 보냄

	// 해당 버퍼의 데이터를 recver_addr 소켓 정보로 보냄

	cout << "파일 크기 : " << file_size << " Byte" << endl;

	cout << "\nUploading ";

	start = clock();
	// 전송 루프
	buf = new char[BUFSIZE];


	while (1) {
		ZeroMemory(buf, _msize(buf));

		// _msize : 동적 할당한 변수에 대하여 크기를 알아오는 함수

		send_size = fread(buf, 1, _msize(buf), filepointer);	
		/// filepointer 에서 총 _msize(buf) 개를 1바이트씩 읽어온다.
		/// buf 총 파일 저장 버퍼에다 씀
		// return file size

		sendto(sock, buf, _msize(buf), 0, (SOCKADDR*)&recver_addr, sizeof(recver_addr));

		// 파일의 내용이 담긴 버퍼를 전송
		// 즉 파일을 전송



		// 응답 수신
		recvfrom(sock, msgbuf, sizeof(msgbuf), 0, NULL, 0);
		cout <<	 msgbuf << " ";

		file_size -= send_size;
		if (file_size == 0) break;

		// 나머지 전송
		if (file_size < BUFSIZE) {
			delete[] buf;
			buf = NULL;
			buf = new char[file_size];
		}


		/// 남은 파일 크기에 맞게 버퍼를 다시 세팅하고 있음
		/// 개선 시도

	}

	end = clock();
	fclose(filepointer);

	delete[] buf;
	buf = NULL;

	// 전송 결과
	cout << "\n\n파일전송 성공 (Time : " << (double)(end - start) << ")\n" << endl;


}