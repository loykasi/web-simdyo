export interface Pagination<T> {
  total: number;
  size: number;
  lastId: number | null;
  items: T[];
}
