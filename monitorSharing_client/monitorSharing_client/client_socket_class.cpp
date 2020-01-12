#include "client_socket_class.h"
#include <assert.h>
#include <time.h>
#include <Windows.h>

client_socket_class::client_socket_class() {
	WSAStartup(MAKEWORD(2, 2), &wsaData);

	/// 버전이 지원하는 Word 크기로, 0000001000000010 =  514
	/// wsaData : Window 소켓 구현의 세부사항을 수신하는 WSADATA 데이터 구조 포인터



	if ((sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_UDP)) == -1) {
		temp = WSAGetLastError(); 
		printf("Socket Error: %x(%d)\n", temp, temp); 
		assert(sock != INVALID_SOCKET);

		/// 소켓이 생성되지 않았을 경우

	}

	//UDP 전송으로 설정


	
	memset(&sock_addr, 0, sizeof(sock_addr));
	// 해당 소켓 주소 객체를 0으로 초기화

	sock_addr.sin_family = AF_INET; //IPv4

	sock_addr.sin_addr.s_addr = htonl(INADDR_ANY); //Host to network long


	/// sock_addr : 소켓 설정하는 구조체

	
	
	/// 연결 주소 : 0.0.0.0
	/// 로컬 시스템의 모든 IPv4 주소
	/// 어떤 IP든 상관 없으니 포트가 일치하면 그 아이피로 접속을 하겠다.
	
	sock_addr.sin_port = htons(PORT); //short 형으로
	
	/// htonl (Host to network long)
	/// htons (Host to Network short)

	/// sock_addr 객체를 사용할시 필요한 사항, 다음과 같이 변환이 필요
	/// 네트워크에서는 빅엔디언 방식의 데이터 저장 방식을 사용
	/// 현재 코딩 환경(CPU) 에서는 리틀 엔디언 방식, 즉 네트워크 방식의 바이트 순서를 맞춰줘야됨

	bind(sock, (sockaddr*)&sock_addr, sizeof(sock_addr));
	/// sock 변수에다 해당 정보로 소켓 생성


	join_addr.imr_multiaddr.s_addr = inet_addr("Server IP 입력");
	// 멀티캐스트를 사용하기 위한 서버의 IP 주소 입력
	// 위에꺼 성공
	// 위에 한글 란은 멀티캐스트를 사용하기 위한 서버 IP 란이 들어가는 곳

	//join_addr.imr_multiaddr.s_addr = htonl(INADDR_ANY);

	// join_addr : 멀티캐스트 그룹에 가입하기 위한 구조체

	join_addr.imr_interface.s_addr = htonl(INADDR_ANY);
	// 해당 그룹에 가입할 IP 주소, 즉 자기 자신을 의미
	// 자신의 컴퓨터에서 사용 가능한 IP를 통해서 해당 서버에 접속하겠다는 의미


	setsockopt(sock, IPPROTO_IP, IP_ADD_MEMBERSHIP, (char*)&join_addr, sizeof(join_addr));


	cout << "Wait for a response.";
}

// 클라이언트 객체가 생성이 될 시 바로 

client_socket_class::~client_socket_class() {
	closesocket(sock);
	WSACleanup();
}

void client_socket_class::connect_server() {
	ZeroMemory(var_connect, sizeof(var_connect));
	// var_connect : 버퍼 (char 형 버퍼), 문자열을 받기 위한 버퍼 (s)

	// 해당 공간에 해당 크기만큼 비워버림, 즉 쓰레기 값 조차도 없애버림

	recvfrom(sock, var_connect, sizeof(var_connect), 0, (SOCKADDR*)&sender_addr, &sender_addr_size);


	/// recvfrom : 데이터 그램 수신, 소스 주소 저장f
	/// sock 소켓으로 들어오는 데이터를 var_connect 버퍼에 저장하고 해당 클라이언트 주소 (받고 있는 자의 정보) 정보를 sender_addr 에다 저장
		

	sendto(sock, var_connect, sizeof(var_connect), 0, (SOCKADDR*)&sender_addr, sock_addr_size);

	/// 소켓을 통해 받은 데이터를 (var_connect 버퍼에 있는 데이터를) sender_addr 클라이언트로 보냄

	/// 즉 버퍼를 거쳐 자신, 즉 클라이언트로 받는 것

}

void client_socket_class::recvfile() {
	clock_t start, end;

	if (recvfrom(sock, (char*)&file_size, sizeof(file_size), 0, NULL, 0) == -1) {
		temp = WSAGetLastError();
		printf("Socket Error: %x(%d)\n", temp, temp);
	}
	system("cls");
	cout << "파일 크기 : " << file_size << " Byte" << endl;

	// 파일 전체 크기의 버퍼 생성
	total_size = 0;
	totalbuf = new char[file_size];
	ZeroMemory(totalbuf, _msize(totalbuf));


	cout << "\nDownloading ";

	start = clock(); // 해당 프로세스가 시스템에서 소비한 시간
	
	// 수신 루프
	buf = new char[BUFSIZE];


	while (1) {
		ZeroMemory(buf, _msize(buf));
		recv_size = recvfrom(sock, buf, _msize(buf), 0, NULL, 0);
		if (recv_size == SOCKET_ERROR) {
			/// 받은 게 없으면

			temp = WSAGetLastError();
			printf("Socket Error: %x(%d)\n", temp, temp);
		}
		
		memcpy(&totalbuf[total_size], buf, recv_size);
		total_size += recv_size;
		cout << ". ";

		// 응답 전송
		if (sendto(sock, msgbuf, sizeof(msgbuf), 0, (SOCKADDR*)&sender_addr, sizeof(sender_addr)) == SOCKET_ERROR) {
			temp = WSAGetLastError();
			printf("Socket Error: %x(%d)\n", temp, temp);
		}
		
		file_size -= recv_size;
		if (file_size == 0) break;

		// 한번에 60000 정보를 보내기 때문에 위와 같은 과정이 필요


		// 나머지 수신
		if (file_size < BUFSIZE) {
			delete[] buf;
			buf = NULL;
			buf = new char[file_size];
		}
	}
	end = clock(); // 해당 프로세스가 현재 시점까지 소비한 시간 반환


	// 파일 열기
	filepointer = fopen("ScreenCapture.jpeg", "wb");
	fwrite(totalbuf, 1, _msize(totalbuf), filepointer);
	fclose(filepointer);

	delete[] buf;
	buf = NULL;
	delete[] totalbuf;
	totalbuf = NULL;

	// 수신 결과
	cout << "\n\n파일수신 완료 (Time : " << (double)(end - start) << ")" << endl;


}