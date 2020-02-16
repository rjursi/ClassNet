#include <stdio.h>

#include <curl/curl.h>

#include <opencv/cv.h>
#include <opencv/highgui.h>

using namespace cv;

struct FtpFile {
	const char *filename;
	FILE *stream;
};

static size_t my_fwrite(void *buffer, size_t size, size_t nmemb, void *stream)
{
	struct FtpFile *out = (struct FtpFile *)stream;
	if (!out->stream) {
		/* open file for writing */
		out->stream = fopen(out->filename, "wb");
		if (!out->stream)
			return -1; /* failure, can't open file to write */
	}
	return fwrite(buffer, size, nmemb, out->stream);
}

void image_download() {
	CURL *curl;
	CURLcode res;
	struct FtpFile ftpfile = {
	  "ScreenCapture.jpeg", /* name to store the file as if successful */
	  NULL
	};

	curl_global_init(CURL_GLOBAL_DEFAULT);

	curl = curl_easy_init();
	if (curl) {
		/*
		 * You better replace the URL with one that works!
		 */
		curl_easy_setopt(curl, CURLOPT_URL,
			"ftp://192.168.31.218:9999/ScreenCapture.jpeg");
		/* Define our callback to get called when there's data to be written */
		curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, my_fwrite);
		/* Set a pointer to our struct to pass to the callback */
		curl_easy_setopt(curl, CURLOPT_WRITEDATA, &ftpfile);

		/* Switch on full protocol/debug output */
		curl_easy_setopt(curl, CURLOPT_VERBOSE, 1L);

		res = curl_easy_perform(curl);

		/* always cleanup */
		curl_easy_cleanup(curl);

		if (CURLE_OK != res) {
			/* we failed */
			fprintf(stderr, "curl told us %d\n", res);
		}
	}

	if (ftpfile.stream)
		fclose(ftpfile.stream); /* close the local file */

	curl_global_cleanup();
}

void image_viewer() {
	String filename = "ScreenCapture.jpeg";

	Mat origin_img = imread(filename, CV_LOAD_IMAGE_COLOR);
	Mat resizing_img;

	resize(origin_img, resizing_img, Size(GetSystemMetrics(SM_CXSCREEN) / 2, GetSystemMetrics(SM_CYSCREEN) / 2), 0, 0, CV_INTER_LINEAR);
	imshow("viewer", resizing_img);

	waitKey(1);
}

int main(void)
{
	while (1) {
		system("cls");
		image_download();
		image_viewer();
	}

	return 0;
}