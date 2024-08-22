import "./CodeBlock.css";

interface Props {
  content: string;
}

const CodeBlock = ({ content }: Props) => {
  return (
    <div className="rounded-div p-3">
      <pre>
        <code>{content}</code>
      </pre>
    </div>
  );
};

export default CodeBlock;
