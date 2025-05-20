import React, { useRef } from "react";
import { Canvas, useFrame } from "react-three-fiber";
import { Html, OrbitControls, Plane, Svg } from "@react-three/drei";
import { Text } from "@react-three/drei";

interface ThreeDGraphicsProps {}

export const ThreeDGraphics: React.FC<ThreeDGraphicsProps> = ({}) => {
  return (
    <Canvas shadows>
      <OrbitControls />
      <RotatingCube />
    </Canvas>
  );
};

type RotatingCubeProps = {};

const RotatingCube: React.FC<RotatingCubeProps> = () => {
  const cubeRef = useRef();

  useFrame(() => {
    if (cubeRef.current) {
      (cubeRef.current as any).rotation.y += 0.01;
      // (cubeRef.current as any).rotation.y += 0.01;
    }
  });

  return (
    <group position={[0, -0.9, -3]}>
      <mesh
        receiveShadow
        castShadow
        rotation-x={-Math.PI / 2}
        position-z={2}
        scale={[4, 20, 0.2]}
      >
        <boxGeometry />
        <meshStandardMaterial color="hotpink" />
      </mesh>
      <mesh
        receiveShadow
        castShadow
        rotation-x={-Math.PI / 2}
        position-y={1}
        scale={[4.2, 0.2, 4]}
      >
        <boxGeometry />
        <meshStandardMaterial color="#e4be00" />
      </mesh>
      <mesh
        receiveShadow
        castShadow
        rotation-x={-Math.PI / 2}
        position={[-1.7, 1, 3.5]}
        scale={[0.5, 4, 4]}
      >
        <boxGeometry />
        <meshStandardMaterial color="#736fbd" />
      </mesh>
      {/* <mesh
        receiveShadow
        castShadow
        rotation-x={-Math.PI / 2}
        position={[0, 4.5, 3]}
        scale={[2, 0.03, 4]}
      >
        <boxGeometry />
        <meshStandardMaterial color="white" />
      </mesh> */}
      <Light />
    </group>
    // <group position={[0, 0, 0]} rotation={[0, Math.PI / 4, 0]}>
    //   <mesh>
    //     <boxGeometry args={[1, 2, 0.3]} />
    //     <meshStandardMaterial color="blue" transparent />

    //     {/* <Plane args={[1, 2]} position={[-0.5, 1, 0.15]} rotation={[0, 0, 0]}>
    //       <Html>
    //         <img
    //           src="http://localhost:3013/images/india/virat-kohli-253802.png"
    //           alt="virat-kohli"
    //           style={{ width: 100, height: 150 }}
    //         />
    //       </Html>
    //     </Plane> */}
    //     <Light />
    //   </mesh>
    // </group>
  );
};

const Light: React.FC = () => {
  const pointlightPosition = 1.5;
  return (
    <>
      <directionalLight position={[5, 5, 5]} intensity={1} />
      <ambientLight intensity={1} />
      {/* <pointLight
        position={[pointlightPosition, pointlightPosition, pointlightPosition]}
        intensity={5}
        distance={10}
      /> */}
      {/* <pointLight
        position={[-pointlightPosition, pointlightPosition, pointlightPosition]}
        intensity={5}
        distance={10}
      />
      <pointLight
        position={[pointlightPosition, -pointlightPosition, pointlightPosition]}
        intensity={5}
        distance={10}
      />
      <pointLight
        position={[pointlightPosition, pointlightPosition, -pointlightPosition]}
        intensity={5}
        distance={10}
      />
      <pointLight
        position={[
          -pointlightPosition,
          -pointlightPosition,
          pointlightPosition,
        ]}
        intensity={5}
        distance={10}
      />
      <pointLight
        position={[
          pointlightPosition,
          -pointlightPosition,
          -pointlightPosition,
        ]}
        intensity={5}
        distance={10}
      /> */}
    </>
  );
};

// interface PlayerImageDemoProps {
//   playerName: string;
//   playerHref: string;
//   src: string;
// }

// export const PlayerImageDemo: React.FC<PlayerImageDemoProps> = ({
//   playerName,
//   playerHref,
// }) => {
//   const { gridSide, margin, height, width } = {
//     margin: 0,
//     height: 600,
//     width: 450,
//     gridSide: 30,
//   };
//   return (
//     <svg
//       width={width}
//       height={height}
//       style={{ background: "none", margin, border: "none" }}
//     >
//       {/* <g className="grid">
//         {Array.from(
//           { length: height / gridSide - 1 },
//           (_, index) => index + 1
//         ).map((x) => (
//           <line
//             x1={0}
//             y1={x * gridSide}
//             x2={width}
//             y2={x * gridSide}
//             stroke="black"
//             strokeOpacity={0.3}
//           />
//         ))}
//         {Array.from(
//           { length: width / gridSide - 1 },
//           (_, index) => index + 1
//         ).map((x) => (
//           <line
//             x1={x * gridSide}
//             y1={0}
//             x2={x * gridSide}
//             y2={height}
//             stroke="black"
//             strokeOpacity={0.3}
//           />
//         ))}
//       </g> */}
//       <defs>
//         <linearGradient id="imageGradient" x1="0%" y1="0%" x2="0%" y2="100%">
//           <stop offset="0%" style={{ stopColor: "pink", stopOpacity: 0.5 }} />
//           <stop offset="100%" style={{ stopColor: "red", stopOpacity: 1 }} />
//         </linearGradient>
//       </defs>
//       <path
//         d="M30,30 Q100,0 250,30 Q325,50 420,35 V570 H30 Z"
//         stroke="darkblue"
//         strokeWidth="5"
//         fill="url(#imageGradient)"
//       />
//       <image
//         xlinkHref="http://localhost:3012/images/team-logos/india.png"
//         width="420"
//         height="600"
//         x="0"
//         y="0"
//       />
//       <image
//         xlinkHref={`http://localhost:3012/images/india/${
//           playerHref?.split("/")[2]
//         }.png`}
//         width="390"
//         height="560"
//         x="30"
//         y="30"
//         style={{
//           filter: "url(#imageGradient)",
//         }}
//       />
//       <svg>
//         <defs>
//           <filter id="drop-shadow" x="-50%" y="-50%" width="200%" height="200%">
//             <feOffset result="offOut" in="SourceAlpha" dx="5" dy="5" />
//             <feGaussianBlur result="blurOut" in="offOut" stdDeviation="3" />
//             <feBlend in="SourceGraphic" in2="blurOut" mode="normal" />
//           </filter>
//         </defs>
//         <path
//           id="arc"
//           d="M30,625 A225,60 0 1,1 420,625"
//           fill="none"
//           stroke="red"
//         />
//         <text
//           x="65%"
//           y="90%"
//           font-size="50"
//           fontWeight="700"
//           font-family="Impact"
//           fill="white"
//           text-anchor="middle"
//           stroke="black"
//           stroke-width="2"
//           filter="url(#drop-shadow)"
//         >
//           <textPath href="#arc">{playerName.toUpperCase()}</textPath>
//         </text>
//       </svg>
//     </svg>
//   );
// };
