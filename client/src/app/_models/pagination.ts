export interface Pagination {
  cuurentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginationResult<T> {
  result?: T;
  pagination?: Pagination;
}
