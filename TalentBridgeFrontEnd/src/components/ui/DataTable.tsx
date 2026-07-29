import React from 'react';

interface Column<T> {
  header: string;
  accessor: (row: T) => React.ReactNode;
}

interface DataTableProps<T> {
  data: T[];
  columns: Column<T>[];
  rowKey: (row: T) => string;
  onRowClick?: (row: T) => void;
  checkboxSelection?: boolean;
}

export function DataTable<T>({ data, columns, rowKey, onRowClick, checkboxSelection }: DataTableProps<T>) {
  return (
    <div className="table-container">
      <table>
        <thead>
          <tr>
            {checkboxSelection && <th style={{ width: '40px' }}><input type="checkbox" /></th>}
            {columns.map((col, i) => (
              <th key={i}>{col.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row) => (
            <tr key={rowKey(row)} onClick={() => onRowClick && onRowClick(row)} style={{ cursor: onRowClick ? 'pointer' : 'default' }}>
              {checkboxSelection && <td><input type="checkbox" /></td>}
              {columns.map((col, i) => (
                <td key={i}>{col.accessor(row)}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
