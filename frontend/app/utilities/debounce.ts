export function debounce<T extends any[]>(
  func: (...args: T) => any,
  delay: number = 500,
): (...args: T) => void {
  let timer: NodeJS.Timeout;
  return (...args: T) => {
    if (timer) {
      clearTimeout(timer);
    }
    timer = setTimeout(() => func(...args), delay);
  };
}
