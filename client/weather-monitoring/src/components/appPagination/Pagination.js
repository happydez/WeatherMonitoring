import React, { useState, useEffect, useCallback } from 'react';
import './appPagination.scss';

const Pagination = ({ totalCities, citiesPerPage, currentPage, paginate }) => {
  const [visiblePages, setVisiblePages] = useState([]);
  const totalPages = Math.max(Math.ceil(totalCities / citiesPerPage), 1);

  const updateVisiblePages = useCallback((newCurrentPage) => {
    let newVisiblePages = [];
    if (totalPages <= 4) {
      newVisiblePages = Array.from({ length: totalPages }, (_, i) => i + 1);
    } else {
      if (newCurrentPage <= 2) {
        newVisiblePages = [1, 2, 3, 4];
      } else if (newCurrentPage >= totalPages - 1) {
        newVisiblePages = [
          totalPages - 3,
          totalPages - 2,
          totalPages - 1,
          totalPages,
        ];
      } else {
        newVisiblePages = [
          newCurrentPage - 1,
          newCurrentPage,
          newCurrentPage + 1,
          newCurrentPage + 2,
        ];
      }
    }
    setVisiblePages(newVisiblePages);
  }, [totalPages]);

  useEffect(() => {
    updateVisiblePages(currentPage);
  }, [currentPage, totalPages, updateVisiblePages]);

  const handlePageChange = (pageNumber) => {
    paginate(pageNumber);
    updateVisiblePages(pageNumber);
  };

  const handlePrevClick = () => {
    if (currentPage > 1) {
      handlePageChange(currentPage - 1);
    }
  };

  const handleNextClick = () => {
    if (currentPage < totalPages) {
      handlePageChange(currentPage + 1);
    }
  };

  return (
    <nav>
      <ul className="pagination">
        <li className={`page-item arrow ${currentPage === 1 ? 'disabled' : ''}`}>
          <button onClick={handlePrevClick} className="page-link">
            &laquo;
          </button>
        </li>
        {visiblePages.map((number) => (
          <li key={number} className={`page-item ${currentPage === number ? 'active' : ''}`}>
            <button onClick={() => handlePageChange(number)} className="page-link">
              {number}
            </button>
          </li>
        ))}
        <li className={`page-item arrow ${currentPage === totalPages ? 'disabled' : ''}`}>
          <button onClick={handleNextClick} className="page-link">
            &raquo;
          </button>
        </li>
      </ul>
    </nav>
  );
};

export default Pagination;
