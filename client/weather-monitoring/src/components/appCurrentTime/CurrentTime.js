import React, { useEffect, useState } from 'react';
import moment from 'moment-timezone';

const CurrentTime = ({ timeZone }) => {
  const [currentTime, setCurrentTime] = useState(moment().tz(timeZone).format('HH:mm:ss'));

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentTime(moment().tz(timeZone).format('HH:mm:ss'));
    }, 1000);

    return () => clearInterval(interval);
  }, [timeZone]);

  return <div>{currentTime}</div>;
};

export default CurrentTime;
