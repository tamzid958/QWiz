import { InfinitySpin } from "react-loader-spinner";

const Loader = () => (
  <div className="h-screen flex items-center justify-center">
    <InfinitySpin
      visible={true}
      color="rgb(30 58 138)"
      ariaLabel="infinity-spin-loading"
    />
  </div>
);

export default Loader;
