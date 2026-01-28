export async function usePreSignedUpload(
  url: string,
  contentType: string,
  file: File | undefined,
  progress: globalThis.Ref<number, number>
) {
  if (url === "" || !file) return;
  return new Promise((resolve, reject) => {
    const xhr = new XMLHttpRequest();
    xhr.open("PUT", url, true);
    xhr.setRequestHeader("Content-Type", contentType);

    xhr.upload.onprogress = (event) => {
      if (event.lengthComputable) {
        progress.value = (event.loaded / event.total) * 100;
      }
    }

    xhr.onload = () => {
      if (xhr.status >= 200 && xhr.status < 300) {
        resolve(xhr.response);
      } else {
        reject(new Error(`Upload failed with status ${xhr.status}`));
      }
    }

    xhr.onerror = () => reject(new Error("Network error occurred"));

    xhr.send(file);
  })
}