import React, { useState, useEffect, useCallback } from 'react';
import './appPagination.scss';

const Pagination = ({ totalItems, itemsPerPage, currentPage, paginate }) => {
  const [visiblePages, setVisiblePages] = useState([]);
  // const [inputPage, setInputPage] = useState(currentPage);
  const totalPages = Math.max(Math.ceil(totalItems / itemsPerPage), 1);

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
    // setInputPage(pageNumber);
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

  const handleFirstClick = () => {
    handlePageChange(1);
  };

  const handleLastClick = () => {
    handlePageChange(totalPages);
  };

  // const handleInputChange = (e) => {
  //   const value = e.target.value;
  //   if (value === '' || (Number(value) > 0 && Number(value) <= totalPages)) {
  //     setInputPage(value);
  //   }
  // };

  // const handleInputBlur = () => {
  //   if (inputPage !== '' && Number(inputPage) !== currentPage) {
  //     handlePageChange(Number(inputPage));
  //   } else {
  //     setInputPage(currentPage);
  //   }
  // };

  return (
    <nav>
      <ul className="pagination">
        <li className={`page-item arrow ${currentPage === 1 ? 'disabled' : ''}`}>
          <button onClick={handleFirstClick} className="page-link">
            &laquo;&laquo;
          </button>
        </li>
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
        <li className={`page-item arrow ${currentPage === totalPages ? 'disabled' : ''}`}>
          <button onClick={handleLastClick} className="page-link">
            &raquo;&raquo;
          </button>
        </li>
        {/* <li className="page-item">
          <input
            type="number"
            value={inputPage}
            onChange={handleInputChange}
            onBlur={handleInputBlur}
            className="page-input"
            min="1"
            max={totalPages}
          />
        </li> */}
      </ul>
    </nav>
  );
};

export default Pagination;
